﻿using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BookFast.ReliableEvents.DistributedMutex
{
    /// <summary>
    /// adopted from https://github.com/mspnp/cloud-design-patterns/blob/master/leader-election/DistributedMutex/BlobDistributedMutex.cs
    /// </summary>
    internal class BlobDistributedMutex
    {
        private static readonly TimeSpan RenewInterval = TimeSpan.FromSeconds(45);
        private static readonly TimeSpan AcquireAttemptInterval = TimeSpan.FromSeconds(65);

        private readonly BlobSettings blobSettings;
        private readonly Func<CancellationToken, Task> taskToRunWhenLeaseAcquired;

        private readonly ILogger logger;

        public BlobDistributedMutex(BlobSettings blobSettings, Func<CancellationToken, Task> taskToRunWhenLeaseAquired, ILogger logger)
        {
            this.blobSettings = blobSettings;
            this.taskToRunWhenLeaseAcquired = taskToRunWhenLeaseAquired;
            this.logger = logger;
        }

        public Task RunTaskWhenMutexAcquired(CancellationToken token)
        {
            var leaseManager = new BlobLeaseManager(blobSettings, logger);
            return RunTaskWhenBlobLeaseAcquired(leaseManager, token);
        }

        private async Task CancelAllWhenAnyCompletes(Task leaderTask, Task renewLeaseTask, CancellationTokenSource cts)
        {
            await Task.WhenAny(leaderTask, renewLeaseTask);

            // Cancel the user's leader task or the renewLease Task, as it is no longer the leader.
            cts.Cancel();

            var allTasks = Task.WhenAll(leaderTask, renewLeaseTask);
            try
            {
                await Task.WhenAll(allTasks);
            }
            catch (Exception)
            {
                if (allTasks.Exception != null)
                {
                    allTasks.Exception.Handle(ex =>
                    {
                        if (!(ex is OperationCanceledException))
                        {
                            logger.LogError(ex.ToString());
                        }

                        return true;
                    });
                }
            }
        }

        private async Task RunTaskWhenBlobLeaseAcquired(BlobLeaseManager leaseManager, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // Try to acquire the blob lease, otherwise wait for some time before we can try again.
                var leaseId = await TryAcquireLeaseOrWait(leaseManager, token);

                if (!string.IsNullOrEmpty(leaseId))
                {
                    // Create a new linked cancellation token source, so if either the 
                    // original token is canceled or the lease cannot be renewed,
                    // then the leader task can be canceled.
                    using (var leaseCts = CancellationTokenSource.CreateLinkedTokenSource(token))
                    {
                        // Run the leader task.
                        var leaderTask = taskToRunWhenLeaseAcquired.Invoke(leaseCts.Token);

                        // Keeps renewing the lease in regular intervals. 
                        // If the lease cannot be renewed, then the task completes.
                        var renewLeaseTask = KeepRenewingLease(leaseManager, leaseId, leaseCts.Token);

                        // When any task completes (either the leader task or when it could
                        // not renew the lease) then cancel the other task.
                        await CancelAllWhenAnyCompletes(leaderTask, renewLeaseTask, leaseCts);
                    }
                }
            }
        }

        private async Task<string> TryAcquireLeaseOrWait(BlobLeaseManager leaseManager, CancellationToken token)
        {
            try
            {
                var leaseId = await leaseManager.AcquireLeaseAsync(token);
                if (!string.IsNullOrEmpty(leaseId))
                {
                    return leaseId;
                }

                await Task.Delay(AcquireAttemptInterval, token);
                return null;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        private async Task KeepRenewingLease(BlobLeaseManager leaseManager, string leaseId, CancellationToken token)
        {
            var renewOffset = new Stopwatch();

            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Immediately attempt to renew the lease
                    // We cannot be sure how much time has passed since the lease was actually acquired
                    renewOffset.Restart();
                    var renewed = await leaseManager.RenewLeaseAsync(leaseId, token);
                    renewOffset.Stop();

                    if (!renewed)
                    {
                        return;
                    }

                    // We delay based on the time from the start of the last renew request to ensure
                    var renewIntervalAdjusted = RenewInterval - renewOffset.Elapsed;

                    // If the adjusted interval is greater than zero wait for that long
                    if (renewIntervalAdjusted > TimeSpan.Zero)
                    {
                        await Task.Delay(RenewInterval - renewOffset.Elapsed, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    await leaseManager.ReleaseLeaseAsync(leaseId);
                }
            }
        }
    }
}

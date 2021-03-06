using BookFast.SeedWork;
using BookFast.Rest;
using BookFast.ServiceFabric.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System.Fabric;

namespace BookFast.Booking.Client.Composition
{
    public class CompositionModule : ICompositionModule
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiOptions>(configuration.GetSection("BookingApi"));

            services.AddSingleton(new FabricClient());

            services.AddSingleton<ICommunicationClientFactory<CommunicationClient<IBookFastBookingAPI>>>(
                serviceProvider => new BookingCommunicationClientFactory(
                    new ServicePartitionResolver(() => serviceProvider.GetService<FabricClient>()),
                    serviceProvider.GetService<IApiClientFactory<IBookFastBookingAPI>>()));

            services.AddSingleton<IPartitionClientFactory<CommunicationClient<IBookFastBookingAPI>>, BookingPartitionClientFactory>();
            services.AddSingleton<IApiClientFactory<IBookFastBookingAPI>, BookingApiClientFactory>();
        }
    }
}

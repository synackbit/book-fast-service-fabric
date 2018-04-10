﻿// <auto-generated />
using BookFast.Facility.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BookFast.Facility.Data.Migrations
{
    [DbContext(typeof(FacilityContext))]
    partial class FacilityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("Relational:Sequence:fm.accommodationseq", "'accommodationseq', 'fm', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:fm.facilityseq", "'facilityseq', 'fm', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BookFast.Facility.Data.Models.Accommodation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "accommodationseq")
                        .HasAnnotation("SqlServer:HiLoSequenceSchema", "fm")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Description");

                    b.Property<int>("FacilityId");

                    b.Property<string>("Images");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(320);

                    b.Property<int>("RoomCount");

                    b.HasKey("Id");

                    b.HasIndex("FacilityId");

                    b.ToTable("Accommodations","fm");
                });

            modelBuilder.Entity("BookFast.Facility.Data.Models.Facility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:HiLoSequenceName", "facilityseq")
                        .HasAnnotation("SqlServer:HiLoSequenceSchema", "fm")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Description");

                    b.Property<string>("Images");

                    b.Property<double?>("Latitude");

                    b.Property<double?>("Longitude");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(320);

                    b.Property<string>("Owner")
                        .IsRequired();

                    b.Property<string>("StreetAddress");

                    b.HasKey("Id");

                    b.ToTable("Facilities","fm");
                });

            modelBuilder.Entity("BookFast.Facility.Data.Models.Accommodation", b =>
                {
                    b.HasOne("BookFast.Facility.Data.Models.Facility", "Facility")
                        .WithMany("Accommodations")
                        .HasForeignKey("FacilityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Events.Data;

namespace ThAmCo.Events.Data.Migrations
{
    [DbContext(typeof(EventsDbContext))]
    [Migration("20191216151135_EventVenueCode")]
    partial class EventVenueCode
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("thamco.events")
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ThAmCo.Events.Data.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new { Id = 1, Email = "bob@example.com", FirstName = "Robert", Surname = "Robertson" },
                        new { Id = 2, Email = "betty@example.com", FirstName = "Betty", Surname = "Thornton" },
                        new { Id = 3, Email = "jin@example.com", FirstName = "Jin", Surname = "Jellybeans" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<TimeSpan?>("Duration");

                    b.Property<string>("Title");

                    b.Property<string>("TypeId")
                        .IsFixedLength(true);

                    b.Property<string>("VenueReservation");

                    b.HasKey("Id");

                    b.ToTable("Events");

                    b.HasData(
                        new { Id = 1, Date = new DateTime(2018, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), Duration = new TimeSpan(0, 6, 0, 0, 0), Title = "Bob's Big 50", TypeId = "PTY" },
                        new { Id = 2, Date = new DateTime(2018, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), Duration = new TimeSpan(0, 12, 0, 0, 0), Title = "Best Wedding Yet", TypeId = "WED" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.GuestBooking", b =>
                {
                    b.Property<int>("CustomerId");

                    b.Property<int>("EventId");

                    b.Property<bool>("Attended");

                    b.HasKey("CustomerId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("Guests");

                    b.HasData(
                        new { CustomerId = 1, EventId = 1, Attended = true },
                        new { CustomerId = 2, EventId = 1, Attended = false },
                        new { CustomerId = 1, EventId = 2, Attended = false },
                        new { CustomerId = 3, EventId = 2, Attended = false }
                    );
                });

            modelBuilder.Entity("ThAmCo.Events.Data.Staff", b =>
                {
                    b.Property<string>("StaffId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.HasKey("StaffId");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("ThAmCo.Events.Data.GuestBooking", b =>
                {
                    b.HasOne("ThAmCo.Events.Data.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ThAmCo.Events.Data.Event", "Event")
                        .WithMany("Bookings")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

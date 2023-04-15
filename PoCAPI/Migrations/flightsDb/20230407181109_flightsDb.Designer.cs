﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PoCAPI.Data;

#nullable disable

namespace PoCAPI.Migrations.flightsDb
{
    [DbContext(typeof(flightsDbContext))]
    [Migration("20230407181109_flightsDb")]
    partial class flightsDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Flight")
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PoCAPI.Models.Flights", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArrivalAirport")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartureAirport")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("flight", "Flight");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArrivalAirport = "JFK",
                            ArrivalTime = new DateTime(2023, 4, 5, 20, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartureAirport = "LAX",
                            DepartureTime = new DateTime(2023, 4, 5, 13, 30, 0, 0, DateTimeKind.Unspecified),
                            FlightNumber = "DL345",
                            Price = 500
                        },
                        new
                        {
                            Id = 2,
                            ArrivalAirport = "LGA",
                            ArrivalTime = new DateTime(2023, 4, 10, 13, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartureAirport = "ORD",
                            DepartureTime = new DateTime(2023, 4, 10, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            FlightNumber = "UA789",
                            Price = 200
                        },
                        new
                        {
                            Id = 3,
                            ArrivalAirport = "SFO",
                            ArrivalTime = new DateTime(2023, 4, 15, 21, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartureAirport = "SEA",
                            DepartureTime = new DateTime(2023, 4, 15, 18, 30, 0, 0, DateTimeKind.Unspecified),
                            FlightNumber = "AS456",
                            Price = 150
                        },
                        new
                        {
                            Id = 4,
                            ArrivalAirport = "JFK",
                            ArrivalTime = new DateTime(2023, 4, 20, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartureAirport = "FLL",
                            DepartureTime = new DateTime(2023, 4, 20, 6, 0, 0, 0, DateTimeKind.Unspecified),
                            FlightNumber = "B6789",
                            Price = 250
                        },
                        new
                        {
                            Id = 5,
                            ArrivalAirport = "LAX",
                            ArrivalTime = new DateTime(2023, 4, 25, 14, 0, 0, 0, DateTimeKind.Unspecified),
                            DepartureAirport = "MDW",
                            DepartureTime = new DateTime(2023, 4, 25, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            FlightNumber = "WN567",
                            Price = 180
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
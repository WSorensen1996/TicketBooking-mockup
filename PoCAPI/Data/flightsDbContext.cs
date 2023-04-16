using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using PoCAPI.Models;
using PoCAPI.Models.DTO;


namespace PoCAPI.Data
{

    public class flightsDbContext : DbContext
    {

            public flightsDbContext(DbContextOptions<flightsDbContext> options) : base(options) { }

            public DbSet<Flights> flight { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            modelBuilder.HasDefaultSchema("Flight");

            modelBuilder.Entity<Flights>().HasData(

                    new Flights()
                    {
                        Id= 1,
                        FlightNumber = "DL345",
                        DepartureAirport = "LAX",
                        ArrivalAirport = "JFK",
                        DepartureTime = new DateTime(2023, 4, 5, 13, 30, 0),
                        ArrivalTime = new DateTime(2023, 4, 5, 20, 0, 0),
                        Price = 500
                    },

                    new Flights()
                    {
                        Id = 2,
                        FlightNumber = "UA789",
                        DepartureAirport = "ORD",
                        ArrivalAirport = "LGA",
                        DepartureTime = new DateTime(2023, 4, 10, 9, 0, 0),
                        ArrivalTime = new DateTime(2023, 4, 10, 13, 0, 0),
                        Price = 200
                    },

                    new Flights()
                    {
                        Id = 3,
                        FlightNumber = "AS456",
                        DepartureAirport = "SEA",
                        ArrivalAirport = "SFO",
                        DepartureTime = new DateTime(2023, 4, 15, 18, 30, 0),
                        ArrivalTime = new DateTime(2023, 4, 15, 21, 0, 0),
                        Price = 150
                    },

                    new Flights()
                    {
                        Id = 4,
                        FlightNumber = "B6789",
                        DepartureAirport = "FLL",
                        ArrivalAirport = "JFK",
                        DepartureTime = new DateTime(2023, 4, 20, 6, 0, 0),
                        ArrivalTime = new DateTime(2023, 4, 20, 9, 0, 0),
                        Price = 250
                    },


                    new Flights()
                    {
                        Id = 5, 
                        FlightNumber = "WN567",
                        DepartureAirport = "MDW",
                        ArrivalAirport = "LAX",
                        DepartureTime = new DateTime(2023, 4, 25, 10, 0, 0),
                        ArrivalTime = new DateTime(2023, 4, 25, 14, 0, 0),
                        Price = 180
                    }



                );  



        }

    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using PoCAPI.Models;
using PoCAPI.Models.DTO; 


namespace PoCAPI.Data
{

    public class ordersDbContext : DbContext
    {

        public ordersDbContext(DbContextOptions<ordersDbContext> options) : base(options) { }

        public DbSet<Orders> orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Orders");
            modelBuilder.Entity<Orders>().HasData(

                new Orders
                {
                    Id = Guid.NewGuid(),
                    CustomerID = "095d470e-1f5a-477f-b0dd-d11edac2deac",
                    CustomerEmail = "Test@gmail.com",
                    FlightNumber = "DEF456",
                    DepartureTime = new DateTime(2023, 4, 2, 14, 0, 0),
                    ArrivalTime = new DateTime(2023, 4, 2, 16, 0, 0),
                    Ammount = 1,
                    TicketPrice = 200,
                    TotalOrderPrice = 200
                }

                );



        }

    }
}

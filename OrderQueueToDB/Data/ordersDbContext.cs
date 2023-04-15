using Microsoft.EntityFrameworkCore;
using OrderQueueToDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

// NuGet -> add package -> EntityFramework.SQLServer && EntityFramework.Tools

// Tools -> Nuget -> PAckage manager console 
// Retter i migrationen -> bare giv den nyt navn 

//  add-migration ordersDb
//  update-database 

// SQL table is now updated

namespace OrderQueueToDB.Data
{
    public class ordersDbContext : DbContext
    {
        public ordersDbContext(DbContextOptions<ordersDbContext> options) : base(options) { }

        public DbSet<Orders> orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure table name and schema for 'orders' entity
            modelBuilder.Entity<Orders>().ToTable("orders", schema: "Orders");
        }
    }
}
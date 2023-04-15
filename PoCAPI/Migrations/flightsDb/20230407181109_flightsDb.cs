using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PoCAPI.Migrations.flightsDb
{
    /// <inheritdoc />
    public partial class flightsDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Flight");

            migrationBuilder.CreateTable(
                name: "flight",
                schema: "Flight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureAirport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrivalAirport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Flight",
                table: "flight",
                columns: new[] { "Id", "ArrivalAirport", "ArrivalTime", "DepartureAirport", "DepartureTime", "FlightNumber", "Price" },
                values: new object[,]
                {
                    { 1, "JFK", new DateTime(2023, 4, 5, 20, 0, 0, 0, DateTimeKind.Unspecified), "LAX", new DateTime(2023, 4, 5, 13, 30, 0, 0, DateTimeKind.Unspecified), "DL345", 500 },
                    { 2, "LGA", new DateTime(2023, 4, 10, 13, 0, 0, 0, DateTimeKind.Unspecified), "ORD", new DateTime(2023, 4, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), "UA789", 200 },
                    { 3, "SFO", new DateTime(2023, 4, 15, 21, 0, 0, 0, DateTimeKind.Unspecified), "SEA", new DateTime(2023, 4, 15, 18, 30, 0, 0, DateTimeKind.Unspecified), "AS456", 150 },
                    { 4, "JFK", new DateTime(2023, 4, 20, 9, 0, 0, 0, DateTimeKind.Unspecified), "FLL", new DateTime(2023, 4, 20, 6, 0, 0, 0, DateTimeKind.Unspecified), "B6789", 250 },
                    { 5, "LAX", new DateTime(2023, 4, 25, 14, 0, 0, 0, DateTimeKind.Unspecified), "MDW", new DateTime(2023, 4, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), "WN567", 180 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flight",
                schema: "Flight");
        }
    }
}

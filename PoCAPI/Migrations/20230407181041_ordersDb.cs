using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoCAPI.Migrations
{
    /// <inheritdoc />
    public partial class ordersDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Orders");

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ammount = table.Column<int>(type: "int", nullable: false),
                    TicketPrice = table.Column<int>(type: "int", nullable: false),
                    TotalOrderPrice = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Orders",
                table: "orders",
                columns: new[] { "Id", "Ammount", "ArrivalTime", "CreatedDate", "CustomerEmail", "CustomerID", "DepartureTime", "FlightNumber", "TicketPrice", "TotalOrderPrice" },
                values: new object[] { new Guid("e049fc80-8e4a-4b2a-ab8e-7288ed0e0e2d"), 1, new DateTime(2023, 4, 2, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Williamsorensen1996@gmail.com", "095d470e-1f5a-477f-b0dd-d11edac2deac", new DateTime(2023, 4, 2, 14, 0, 0, 0, DateTimeKind.Unspecified), "DEF456", 200, 200 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders",
                schema: "Orders");
        }
    }
}

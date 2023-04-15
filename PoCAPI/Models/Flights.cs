using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoCAPI.Models
{


    public class Flights
    {

        public int Id { get; init; }
        public string FlightNumber { get; init;  }

        public string DepartureAirport { get; init; }

        public string ArrivalAirport { get; init; }

        public DateTime DepartureTime { get; init; }

        public DateTime ArrivalTime { get; init; }

        public int Price { get; init; }

    }
}

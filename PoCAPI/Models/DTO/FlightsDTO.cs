using Microsoft.EntityFrameworkCore;

namespace PoCAPI.Models.DTO
{


    public class FlightsDTO
    {
        public string FlightNumber { get; init; }

        public string DepartureAirport { get; init; }

        public string ArrivalAirport { get; init; }

        public DateTime DepartureTime { get; init; }

        public DateTime ArrivalTime { get; init; }

        public int Price { get; init; }


    }
}

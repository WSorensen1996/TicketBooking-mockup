using System.ComponentModel.DataAnnotations;

namespace PoCAPI.Models.DTO
{
    public class ordersDTO
    {




        public Guid Id { get; init; }

        [Required]
        public string CustomerID { get; init; }

        [Required]
        public string CustomerEmail { get; init; }

        [Required]
        public string FlightNumber { get; init; }

        [Required]
        public DateTime DepartureTime { get; init; }

        [Required]
        public DateTime ArrivalTime { get; init; }

        [Required]
        public int Ammount { get; init; }

        [Required]
        public int TicketPrice { get; init; }

        [Required]
        public int TotalOrderPrice { get; init; }





    }
}

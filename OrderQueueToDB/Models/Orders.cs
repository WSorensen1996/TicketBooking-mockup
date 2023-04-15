using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderQueueToDB.Models
{
    public class Orders
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

        public DateTimeOffset CreatedDate { get; init; }


    }
}

using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OrderQueueToDB.Models;
using OrderQueueToDB.Data;
using Newtonsoft.Json;

namespace OrderQueueToDB
{
    public class FunctionOrderQueue
    {
        private readonly ILogger _logger;
        private readonly ordersDbContext _db;

        public FunctionOrderQueue(ordersDbContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<FunctionOrderQueue>();
        }


        public void SendToDB(List<ordersDTO> ordersDTO_lst)
        {
            foreach (var ordersDTO in ordersDTO_lst)
            {
                // checking if the Item is unique
                if (_db.orders.FirstOrDefault(u => u.Id == ordersDTO.Id) != null)
                {
                    _logger.LogInformation($"Failed order with OrderID {ordersDTO.Id} from customerID {ordersDTO.CustomerID} ({ordersDTO.CustomerEmail})");
                }

                Orders model = new Orders()
                {
                    Id = ordersDTO.Id,
                    CustomerID = ordersDTO.CustomerID,
                    CustomerEmail = ordersDTO.CustomerEmail,
                    FlightNumber = ordersDTO.FlightNumber,
                    DepartureTime = ordersDTO.DepartureTime,
                    ArrivalTime = ordersDTO.ArrivalTime,
                    Ammount = ordersDTO.Ammount,
                    TicketPrice = ordersDTO.TicketPrice,
                    TotalOrderPrice = ordersDTO.TotalOrderPrice,
                    CreatedDate = DateTimeOffset.Now,
                };

                _db.orders.Add(model);
                _db.SaveChanges();
            }
        }


        [Function("Function1")]
        public void Run([ServiceBusTrigger("orderqueuetodb", Connection = "ServiceBusConnection")] string myQueueItem)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            List<ordersDTO> _ordersDTO = JsonConvert.DeserializeObject<List<ordersDTO>>(myQueueItem);
            SendToDB(_ordersDTO); 
        }
    }
}

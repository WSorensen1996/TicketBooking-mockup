using Microsoft.AspNetCore.Mvc;
using PoCAPI.Models.DTO;
using System.Threading.Tasks;
using PoCAPI.Data;
using PoCAPI.Models;
using PoCAPI.Services;
using System.Collections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.Amqp.Framing;

namespace PoCAPI.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ordersDbContext _db;
        private readonly IQueueService _queueService;
        private readonly string _queueName = "orderqueuetodb";
       

        public OrdersController(
            UserManager<ApplicationUser> userManager,
            ordersDbContext db, 
            IQueueService queueService
            )
        {
            _db = db;
            _queueService = queueService;
            _userManager = userManager;

        }



        [HttpGet("{Email}", Name = "Email")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ordersDTO>>> GetOrders(string Email)
        {


            if (string.IsNullOrEmpty(Email))
            {
                return BadRequest();
            }

            var _orders = _db.orders.Where(u => u.CustomerEmail == Email.ToString()).ToList();


            if (_orders == null)
            {
                return NotFound();
            }

            return Ok(_orders);

        }



        [HttpPost]
        public async Task<ActionResult<Orders>> CreateOrder([FromBody] List<ordersDTO> ordersDTO)
        {


            foreach (var _order in ordersDTO)
            {

                // checking if the Item is unique
                if (_db.orders.FirstOrDefault(u => u.Id == _order.Id) != null)
                {
                    ModelState.AddModelError("CustomError", "Order already exists!");
                    return BadRequest(ModelState);
                }

                if (_order == null)
                {
                    return BadRequest(_order);
                }

            }

            // Send the message to the queue
            await _queueService.SendMessageAsync(ordersDTO, _queueName);

  
            return Accepted(ordersDTO);



        }

        }
}

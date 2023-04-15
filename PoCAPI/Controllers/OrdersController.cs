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

    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ordersDbContext _db;
        private readonly IQueueService _queueService;
        private readonly string _queueName = "orderqueuetodb";
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ordersDbContext db, IQueueService queueService, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _queueService = queueService;
            _userManager = userManager;
        }


        private async Task<bool> CheckAuth(string token, string email)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return false;
            }

            var tokenValue = authorizationHeader.Split(" ").LastOrDefault();
            if (string.IsNullOrEmpty(tokenValue) || tokenValue == "Bearer")
            {
                return false;
            }

            var user = await _userManager.FindByEmailAsync(email);
            bool isTokenValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "AccessToken", tokenValue);

            return isTokenValid;
        }


        [HttpGet("{Costumer_id}", Name = "Costumer_id")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ordersDTO>>> GetOrders(string Costumer_id)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var email = Request.Headers["Email"];

            bool isTokenValid = await CheckAuth(token, email);

            if (isTokenValid == false)
            {
                return BadRequest();
            }


            if (string.IsNullOrEmpty(Costumer_id))
            {
                return BadRequest();
            }

            var _orders = _db.orders.Where(u => u.CustomerID == Costumer_id.ToString()).ToList();


            if (_orders == null)
            {
                return NotFound();
            }

            return Ok(_orders);

        }



        [HttpPost]
        public async Task<ActionResult<Orders>> CreateOrder([FromBody] List<ordersDTO> ordersDTO)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var email = Request.Headers["Email"];
            bool isTokenValid = await CheckAuth(token, email); 

            if (isTokenValid == false)
            {
                return BadRequest();
            }


            if (isTokenValid)
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

                //}

                // Returns the url where the data is stored
                return Accepted(ordersDTO);

            }
            else
            {
                return Unauthorized();
            }

        }

        }
}

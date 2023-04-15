using Microsoft.AspNetCore.Mvc;
using PoCWebApp.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using PoCWebApp.Data;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using PoCWebApp.Areas.Identity.Data;
using PoCWebApp.Services;
using System.Net.Http.Headers;

namespace PoCWebApp.Controllers
{

    public class HomeViewModel
    {
        public int totprice { get; set; }

    }
    public class HomeController : Controller
    {


        private readonly IHttpContextAccessor _contextAccessor;
        private readonly List<Dictionary<string, object>> _cartList;
        private readonly List<ordersDTO> _cartListFlights;
        private readonly IConfiguration _configuration;
        private readonly FlightRepository _flightRepository;
        private readonly BookingsRepository _bookingsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, List<Dictionary<string, object>> cartList, List<ordersDTO> cartListFlights)
        {
            _contextAccessor = httpContextAccessor;
            _cartList = cartList;
            _cartListFlights = cartListFlights;
            _configuration = configuration;
            _flightRepository = new FlightRepository(_configuration);
            _bookingsRepository = new BookingsRepository(_configuration);
            _userManager = userManager; 
        }

        public int CalculateTotalPrice()
        {
            int totalPrice = 0;
            foreach (var flightData in _cartList)
            {
                int price;
                if (int.TryParse(flightData["Price"].ToString(), out price))
                {
                    totalPrice += price;
                }
                else
                {
                    // Should handle the error here 
                }
            }
            return totalPrice;
        }


        public IActionResult Index()
        {

            //ViewData["UserId"] = _userManager.GetUserId(this.User);
            //ViewData["Name"] = _userManager.GetUserName(this.User);

            return View();
        }

        public IActionResult OrderError()
        {


            return View();
        }

        [Authorize]
        public IActionResult Claims()
        {


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Bookings()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["UserId"] = _userManager.GetUserId(this.User);
                ViewData["Email"] = _userManager.GetUserName(this.User);
                ViewData["AuthToken"] = HttpContext.Session.GetString("AuthToken"); 

                return View();
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Cart()
        {

            var model = new HomeViewModel
            {
                totprice = CalculateTotalPrice()
            };


            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> CheckoutAsync()
        {

            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToPage("/Account/Logout", new { area = "Identity" });
            }
               
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(this.User));

            var connectionString = _configuration.GetConnectionString("API");


            // Convert the object to JSON
            var json = JsonConvert.SerializeObject(_cartListFlights);


            // Create an instance of HttpClient
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("AuthToken"));
                client.DefaultRequestHeaders.Add("Email", user.Email);

                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(connectionString + "/api/orders", httpContent);

                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.IsSuccessStatusCode)
                {
                    _cartList.Clear();
                    _cartListFlights.Clear();
                    HttpContext.Session.Remove("ShoppingCart");

                    return View();
                }
                else
                {
                    return RedirectToAction("OrderError");
                }


            }

        }







            public IActionResult RemoveFromCart(string FlightNumber)
        {
            // Find the flight in the cart list
            var _cartListflightToRemove = _cartList.FirstOrDefault(flight => flight["FlightNumber"].ToString() == FlightNumber);
            var _cartListFlightsflightToRemove = _cartListFlights.FirstOrDefault(flight => flight.FlightNumber.ToString() == FlightNumber);
            // If the flight is found, remove it from the cart list
            if (_cartListflightToRemove != null)
            {
                _cartList.Remove(_cartListflightToRemove);
                _cartListFlights.Remove(_cartListFlightsflightToRemove);

                // serialize the updated list to JSON
                var jsonData = JsonConvert.SerializeObject(_cartList);

                // store the updated JSON string in the session
                _contextAccessor.HttpContext.Session.SetString("ShoppingCart", jsonData);
            }


            return RedirectToAction("Cart");
        }


        public async Task<IActionResult> AddToCart(string FlightNumber)
        {

            if (User.Identity.IsAuthenticated)
            {

                Flight flight = await _flightRepository.GetFlightById(FlightNumber);




                var _flight = new Dictionary<string, object>();
                _flight.Add("FlightNumber", flight.FlightNumber);
                _flight.Add("DepartureAirport", flight.DepartureAirport);
                _flight.Add("ArrivalAirport", flight.ArrivalAirport);
                _flight.Add("Price", flight.Price);


                ordersDTO _order = new ordersDTO
                {
                    Id = Guid.NewGuid(),
                    CustomerEmail = _userManager.GetUserName(this.User),
                    CustomerID = _userManager.GetUserId(this.User),
                    FlightNumber = flight.FlightNumber,
                    DepartureTime = flight.DepartureTime,
                    ArrivalTime = flight.ArrivalTime,
                    Ammount = 1,
                    TicketPrice = flight.Price,
                    TotalOrderPrice = CalculateTotalPrice(),

                }; 

                _cartListFlights.Add(_order);

                // add the flight data to the cart list
                _cartList.Add(_flight);


                // serialize the list to JSON
                var jsonData = JsonConvert.SerializeObject(_cartList);

                // store the JSON string in the session
                _contextAccessor.HttpContext.Session.SetString("ShoppingCart", jsonData);


                return RedirectToAction("Cart");
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using PoCWebApp.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;
using PoCWebApp.Services;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

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
        private readonly HttpClient _httpClient;


        public HomeController(
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor, 
            List<Dictionary<string, object>> cartList, 
            List<ordersDTO> cartListFlights,
            HttpClient httpClient
            )
        {
            _contextAccessor = httpContextAccessor;
            _cartList = cartList;
            _cartListFlights = cartListFlights;
            _configuration = configuration;
            _flightRepository = new FlightRepository(_configuration);
            _httpClient = httpClient;
            _bookingsRepository = new BookingsRepository(_configuration, _contextAccessor, _httpClient);

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
            return View();
        }

        public IActionResult OrderError()
        {


            return View();
        }

    
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

            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login");
            }

            return View();
            
    

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {

                var connectionString = _configuration.GetConnectionString("API");
                var json = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PostAsync(connectionString + "/api/Auth/register", stringContent);

                if (httpResponse.IsSuccessStatusCode)
                {

                    return RedirectToAction("Login");
                }
                else
                {
                    
                    string errorMessage = await httpResponse.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return View("Register", model);
                }

            }
            else
            {

                ModelState.AddModelError(string.Empty, "Form invalid"); 
                return View("Register", model);
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            _contextAccessor.HttpContext.Session.Clear();
            return View("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var connectionString = _configuration.GetConnectionString("API");

                var json = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
     
                var httpResponse = await _httpClient.PostAsync(connectionString + "/api/Auth/login", stringContent);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();

                    dynamic tokenObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    string token = tokenObject.token;
                    string CostumerId = tokenObject.costumerId;


                    HttpContext.Session.SetString("AuthToken", token);
                    HttpContext.Session.SetString("Email", model.Email);
                    HttpContext.Session.SetString("CostumerID", CostumerId);

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password"); // Add an error message to the ModelState
                    return View("Login", model);

                }
            }

            else
            {

                return View(model);
            }
        }


        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Cart()
        {
            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login");

            }


            var model = new HomeViewModel
            {
                totprice = CalculateTotalPrice()
            };


            return View(model);
        }



        public async Task<IActionResult> CheckoutAsync()
        {

            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login");
            }

            var connectionString = _configuration.GetConnectionString("API");
            var json = JsonConvert.SerializeObject(_cartListFlights);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _contextAccessor.HttpContext.Session.GetString("AuthToken"));
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(connectionString + "/api/orders", httpContent);

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

            public IActionResult RemoveFromCart(string FlightNumber)
        {
            var _cartListflightToRemove = _cartList.FirstOrDefault(flight => flight["FlightNumber"].ToString() == FlightNumber);
            var _cartListFlightsflightToRemove = _cartListFlights.FirstOrDefault(flight => flight.FlightNumber.ToString() == FlightNumber);
            if (_cartListflightToRemove != null)
            {
                _cartList.Remove(_cartListflightToRemove);
                _cartListFlights.Remove(_cartListFlightsflightToRemove);
                var jsonData = JsonConvert.SerializeObject(_cartList);
                _contextAccessor.HttpContext.Session.SetString("ShoppingCart", jsonData);
            }


            return RedirectToAction("Cart");
        }


        public async Task<IActionResult> AddToCart(string FlightNumber)
        {

            if (HttpContext.Session.GetString("AuthToken") == null)
            {
                return RedirectToAction("Login");
            }
            
            else
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
                    CustomerEmail = HttpContext.Session.GetString("Email"),
                    CustomerID = HttpContext.Session.GetString("CostumerID"),
                    FlightNumber = flight.FlightNumber,
                    DepartureTime = flight.DepartureTime,
                    ArrivalTime = flight.ArrivalTime,
                    Ammount = 1,
                    TicketPrice = flight.Price,
                    TotalOrderPrice = CalculateTotalPrice(),

                }; 

                _cartListFlights.Add(_order);
                _cartList.Add(_flight);
                var jsonData = JsonConvert.SerializeObject(_cartList);
                _contextAccessor.HttpContext.Session.SetString("ShoppingCart", jsonData);
                return RedirectToAction("Cart");
            }
            
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
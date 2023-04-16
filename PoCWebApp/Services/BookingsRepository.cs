using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PoCWebApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace PoCWebApp.Services
{

    public class BookingsRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;

        public BookingsRepository(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            HttpClient httpClient
            )
        {
            _connectionString = configuration.GetConnectionString("API");
            _contextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task<List<ordersDTO>> GetBookings()
        {

            string token = _contextAccessor.HttpContext.Session.GetString("AuthToken");
            string Email = _contextAccessor.HttpContext.Session.GetString("Email");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
         

            using var response = await _httpClient.GetAsync(_connectionString + "/api/orders/" + Email);
            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<ordersDTO>>(json);


            return orders;
        }

    }


    
}

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

        public BookingsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("API");

        }

        public async Task<List<ordersDTO>> GetBookings(string CostumerID, string Email, string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("Email", Email);

            using var response = await client.GetAsync(_connectionString + "/api/orders/" + CostumerID);
            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<ordersDTO>>(json);


            return orders;
        }

    }


    
}

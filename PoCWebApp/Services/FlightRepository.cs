using Newtonsoft.Json;
using PoCWebApp.Models;
using System;
using System.Collections.Generic;

namespace PoCWebApp.Services
{
    public class FlightRepository
    {
        private readonly string _connectionString;

        public FlightRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("API");
        }

        public async Task<List<Flight>> GetAllFlights()
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(_connectionString + "/api/flights");
            var json = await response.Content.ReadAsStringAsync();
            var flights = JsonConvert.DeserializeObject<List<Flight>>(json);


            return flights;
        }


        public async Task<Flight> GetFlightById(string FlightID)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(_connectionString + "/api/flights/" + FlightID);
            var json = await response.Content.ReadAsStringAsync();
            var flights = JsonConvert.DeserializeObject<Flight>(json);

            return flights;

        }



    }
}

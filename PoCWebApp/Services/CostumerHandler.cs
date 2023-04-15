using Newtonsoft.Json;
using PoCWebApp.Models;
using System.Text;

namespace PoCWebApp.Services
{
    public class CostumerHandler
    {
        private readonly string _connectionString;

        public CostumerHandler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("API");
        }

        public async Task<List<ordersDTO>> postCostumer(CostumerDTO costumerDto)
        {
            using var httpClient = new HttpClient();

            // Serialize the CostumerDTO object to JSON
            var json = JsonConvert.SerializeObject(costumerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Post the serialized JSON to the API endpoint
            var response = await httpClient.PostAsync(_connectionString + "/api/costumers" , content);

            // Check if the response is successful
            response.EnsureSuccessStatusCode();

            // Read the response content as JSON
            var responseJson = await response.Content.ReadAsStringAsync();

            // Deserialize the response JSON to a List<ordersDTO> object
            var ordersDto = JsonConvert.DeserializeObject<List<ordersDTO>>(responseJson);

            return ordersDto;
        }
    }
}

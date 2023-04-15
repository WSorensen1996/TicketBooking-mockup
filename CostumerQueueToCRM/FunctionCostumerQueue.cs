using System.Net.Http.Headers;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceBusToCRM.Models;


namespace ServiceBusToCRM
{
    public class FunctionCostumerQueue
    {

        private readonly ILogger _logger;

        public FunctionCostumerQueue(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FunctionCostumerQueue>();
        }

        private static void LogRequestAndResponse(string client, string json, HttpResponseMessage response)
        {
            // Should be loggin here
            Console.WriteLine("HTTP POST Request:");
            Console.WriteLine($"  Endpoint: {client}");
            Console.WriteLine($"  Headers: {System.Text.Json.JsonSerializer.Serialize(response.RequestMessage.Headers)}");
            Console.WriteLine($"  Body: {json}");

            Console.WriteLine("HTTP POST Response:");
            Console.WriteLine($"  Status Code: {response.StatusCode}");
            Console.WriteLine($"  Headers: {System.Text.Json.JsonSerializer.Serialize(response.Headers)}");
            Console.WriteLine($"  Body: {response.Content.ReadAsStringAsync().Result}");
        }


        public async Task ApiCall(CostumerDTO contact)
        {
            string client = "https://api.hubapi.com/crm/v3/objects/contacts?limit=10&archived=false";
            var data = new
            {
                properties = new
                {
                    costumerID = contact.CostumerID, 
                    email = contact.Email,
                    firstname = contact.FirstName,
                    lastname = contact.LastName,
                    city = contact.City,
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(data);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(client, new StringContent(json, Encoding.UTF8, "application/json"));
                LogRequestAndResponse(client, json, response);
                if (response.IsSuccessStatusCode)
                {
                 
                    _logger.LogInformation($"C# CRM LOG: {response.StatusCode.ToString()}: {json}");
                }
                else
                {

                    _logger.LogInformation($"C# CRM LOG: {response.StatusCode.ToString()}: {json}");
                }
            }
        }


        [Function("costumerqueuetocrm")]
        public void Run([ServiceBusTrigger("costumerqueuetocrm", Connection = "costumerqueuetocrm")] string myQueueItem)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            CostumerDTO _costumer = JsonConvert.DeserializeObject<CostumerDTO>(myQueueItem);

            ApiCall(_costumer);

        }
    }
}

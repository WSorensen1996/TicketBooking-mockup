using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;

namespace PoCAPI.Services
{
    public class QueueService : IQueueService
    {

        private readonly IConfiguration _config;

        public QueueService(IConfiguration config)
        {
            _config = config;
        }


        public async Task SendMessageAsync<T>(T serviceBusMessage, string queueName)
        {

            var queueClient = new QueueClient(_config.GetConnectionString("AzureServiceBus"), queueName);
            string messagebody = JsonSerializer.Serialize(serviceBusMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messagebody));

            await queueClient.SendAsync(message);

        }


    }

}

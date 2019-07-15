using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using VDS.WPS.Interfaces;
using VDS.WPS.Logging;
using VDS.WPS.Settings;

namespace VDS.WPS.Services
{
    public class ServiceBusService : IServiceBusService
    {
        private readonly IQueueClient _queueClient;
        private readonly IOptions<ServiceBusSettings> _serviceBusSettings;
        private readonly LoggerAdapter<ServiceBusService> _logger;

        public ServiceBusService(
            IQueueClient queueClient,
            IOptions<ServiceBusSettings> serviceBusSettings,
            LoggerAdapter<ServiceBusService> logger)
        {
            _queueClient = queueClient;
            _serviceBusSettings = serviceBusSettings;
            _logger = logger;
        }

        public async Task SendMessageAsync(string queueName, Message message)
        {
            _logger.LogInformation("SendMessageAsync params: queueName = {0} & message = {1}", queueName, message);

            QueueClient client = new QueueClient(_serviceBusSettings.Value.ConnectionString, queueName);

            await client.SendAsync(message);
        }
    }
}
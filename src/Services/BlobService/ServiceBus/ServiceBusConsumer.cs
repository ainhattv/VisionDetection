using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VDS.BlobService.Settings;

namespace BlobService.ServiceBus
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        // private readonly IAppLogger<ServiceBusConsumer> _logger;
        private readonly QueueClient _queueClient;
        private readonly IOptions<ServiceBusSettings> _serviceBusSettings;

        public ServiceBusConsumer(
            // IAppLogger<ServiceBusConsumer> logger,
            IOptions<ServiceBusSettings> serviceBusSettings
        )
        {
            // _logger = logger;
            _serviceBusSettings = serviceBusSettings ?? throw new System.ArgumentNullException(nameof(serviceBusSettings));

            _queueClient = new QueueClient(_serviceBusSettings.Value.ConnectionString, "wp2blob");
        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var myPayload = JsonConvert.DeserializeObject<object>(Encoding.UTF8.GetString(message.Body));
            //_processData.Process(myPayload);
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            // _logger.LogError("Message handler encountered an exception");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            // _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            // _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            // _logger.LogDebug($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }

        public async Task CloseQueueAsync()
        {
            await _queueClient.CloseAsync();
        }
    }
}
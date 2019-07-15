using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace VDS.WPS.Interfaces
{
    public interface IServiceBusService
    {
        Task SendMessageAsync(string queueName, Message message);
    }
}
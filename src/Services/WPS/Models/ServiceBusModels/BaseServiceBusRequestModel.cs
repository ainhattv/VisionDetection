namespace WPS.Models.ServiceBusModels
{
    public class BaseServiceBusRequestModel<T>
    {
        public int Id { get; set; }

        public T Body { get; set; }
    }
}
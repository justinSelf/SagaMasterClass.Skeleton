namespace Sales.Messages
{
    using NServiceBus;

    public interface IOrderAbandoned : IEvent
    {
        string OrderId { get; set; } 
    }
}
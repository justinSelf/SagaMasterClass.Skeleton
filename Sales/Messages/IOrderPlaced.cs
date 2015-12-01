namespace Sales.Messages
{
    using NServiceBus;

    public interface IOrderPlaced : IEvent
    {
        string OrderId { get; set; }
    }
}

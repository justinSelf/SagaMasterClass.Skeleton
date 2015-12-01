namespace Sales.Messages
{
    using NServiceBus;

    public interface IOrderCanceled : IEvent
    {
        string OrderId { get; set; }
    }
}

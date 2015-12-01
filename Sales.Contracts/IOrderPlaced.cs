namespace Sales.Contracts
{
    using NServiceBus;

    public interface IOrderPlaced : IEvent
    {
        string OrderId { get; set; }
    }
}

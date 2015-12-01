namespace Sales.Contracts
{
    using NServiceBus;

    public interface IOrderStarted : IEvent
    {
        string OrderId { get; set; }
    }
}

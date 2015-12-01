namespace Sales.Contracts
{
    using NServiceBus;

    public interface IOrderCanceled : IEvent
    {
        string OrderId { get; set; }
    }
}

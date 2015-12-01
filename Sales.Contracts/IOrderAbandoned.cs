namespace Sales.Contracts
{
    using NServiceBus;

    public interface IOrderAbandoned : IEvent
    {
        string OrderId { get; set; } 
    }
}
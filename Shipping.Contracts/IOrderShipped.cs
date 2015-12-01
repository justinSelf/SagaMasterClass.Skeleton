namespace Shipping.Contracts
{
    using NServiceBus;

    public interface IOrderShipped : IEvent
    {
        string OrderId { get; set; } 
    }
}
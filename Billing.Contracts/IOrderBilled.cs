namespace Billing.Contracts
{
    using NServiceBus;

    public interface IOrderBilled : IEvent
    {
        string OrderId { get; set; }
    }
}
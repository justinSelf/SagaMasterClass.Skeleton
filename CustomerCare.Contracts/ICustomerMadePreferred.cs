namespace CustomerCare.Contracts
{
    using NServiceBus;

    public interface ICustomerMadePreferred : IEvent
    {
        string CustomerId { get; set; } 
    }
}
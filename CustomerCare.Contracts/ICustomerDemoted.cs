namespace CustomerCare.Contracts
{
    using NServiceBus;

    public interface ICustomerDemoted : IEvent
    {
        string CustomerId { get; set; } 
    }
}
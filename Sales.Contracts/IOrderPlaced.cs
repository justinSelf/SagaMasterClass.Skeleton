namespace Sales.Contracts
{
    using System;
    using NServiceBus;

    public interface IOrderPlaced : IEvent
    {
        string OrderId { get; set; }
        double OrderValue { get; set; }
        string CustomerId { get; set; }
        DateTime OrderDate { get; set; }
    }
}

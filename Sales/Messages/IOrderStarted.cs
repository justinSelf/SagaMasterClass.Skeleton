﻿namespace Sales.Messages
{
    using NServiceBus;

    public interface IOrderStarted : IEvent
    {
        string OrderId { get; set; }
    }
}

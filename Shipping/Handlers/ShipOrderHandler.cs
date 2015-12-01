namespace Shipping.Handlers
{
    using System;
    using Messages;
    using NServiceBus;

    public class ShipOrderHandler : IHandleMessages<ShipOrder>
    {
        public void Handle(ShipOrder message)
        {
            Console.WriteLine($"Shipping order: {message.OrderId}!");
        }
    }
}
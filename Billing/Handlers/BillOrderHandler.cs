namespace Billing.Handlers
{
    using System;
    using Contracts;
    using Messages;
    using NServiceBus;

    class BillOrderHandler : IHandleMessages<BillOrder>
    {
        IBus Bus;

        public BillOrderHandler(IBus bus)
        {
            Bus = bus;
        }

        public void Handle(BillOrder message)
        {
            Console.WriteLine($"Billing for order {message.OrderId}");
            Bus.Publish<IOrderBilled>(msg => msg.OrderId = message.OrderId);
        }
    }
}

namespace Billing.Handlers
{
    using Messages;
    using NServiceBus;
    using Sales.Contracts;

    public class OrderPlacedHandler : IHandleMessages<IOrderPlaced>
    {
        IBus bus;

        public OrderPlacedHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(IOrderPlaced message)
        {
            //
            bus.SendLocal(new BillOrder
            {
                OrderId = message.OrderId
            });
        }
    }
}
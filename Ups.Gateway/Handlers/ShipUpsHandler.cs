namespace Ups.Gateway.Handlers
{
    using Messages;
    using NServiceBus;

    public class ShipUpsHandler : IHandleMessages<ShipUps>
    {
        IBus Bus;

        public ShipUpsHandler(IBus bus)
        {
            Bus = bus;
        }

        public void Handle(ShipUps message)
        {
            Bus.Reply(new UpsShipResponse {HasShipped = true});
        }
    }
}
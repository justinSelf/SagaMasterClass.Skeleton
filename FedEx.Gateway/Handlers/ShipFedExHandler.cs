namespace FedEx.Gateway.Handlers
{
    using System.Net;
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;

    public class ShipFedExHandler : IHandleMessages<ShipFedEx>
    {
        IBus Bus;

        public ShipFedExHandler(IBus bus)
        {
            Bus = bus;
        }

        public void Handle(ShipFedEx message)
        {
            using (var client = new WebClient())
            {
                var response = client.DownloadString("http://localhost:8888/fedex/shipit");

                var shipResponse = new FedExShipResponse();

                if (response == "") shipResponse.HasShipped = true;

                Bus.Reply(shipResponse);
            }
        }
    }
}
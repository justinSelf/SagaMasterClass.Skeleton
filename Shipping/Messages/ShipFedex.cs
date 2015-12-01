namespace FedEx.Gateway.Messages
{
    using NServiceBus;

    class ShipFedEx : ICommand
    {
        public string OrderId { get; set; }
    }
}

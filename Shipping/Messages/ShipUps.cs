namespace Ups.Gateway.Messages
{
    using NServiceBus;

    class ShipUps : ICommand
    {
        public string OrderId { get; set; }
    }
}

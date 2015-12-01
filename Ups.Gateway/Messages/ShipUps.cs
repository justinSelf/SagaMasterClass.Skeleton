namespace Ups.Gateway.Messages
{
    using NServiceBus;

    public class ShipUps : ICommand
    {
        public string OrderId { get; set; }
    }
}

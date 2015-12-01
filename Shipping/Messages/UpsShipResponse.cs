namespace Ups.Gateway.Messages
{
    using NServiceBus;

    public class UpsShipResponse : IMessage
    { 
        public bool HasShipped { get; set; }
    }
}
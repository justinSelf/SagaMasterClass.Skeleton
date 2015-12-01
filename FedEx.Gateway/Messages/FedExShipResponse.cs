namespace FedEx.Gateway.Messages
{
    using NServiceBus;

    public class FedExShipResponse : IMessage
    {
        public bool HasShipped { get; set; } 
    }
}
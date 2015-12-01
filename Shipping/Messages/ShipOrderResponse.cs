namespace Shipping.Messages
{
    using NServiceBus;

    public class ShipOrderResponse : IMessage
    {
        public bool HasShipped { get; set; } 
    }
}
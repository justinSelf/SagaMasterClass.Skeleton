namespace Shipping.Messages
{
    using NServiceBus;

    public class ShipOrder : ICommand
    {
        public ShipOrder(string orderId)
        {
            OrderId = orderId;
        }
        public string OrderId { get; set; }
    }
}
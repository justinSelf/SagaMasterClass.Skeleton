namespace FedEx.Gateway.Messages
{
    using NServiceBus;

    public class ShipFedEx: ICommand
    {
         public string OrderId { get; set; }
    }
}
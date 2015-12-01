namespace Billing.Messages
{
    using NServiceBus;

    public class BillOrder : ICommand
    {
        public string OrderId { get; set; }
    }
}
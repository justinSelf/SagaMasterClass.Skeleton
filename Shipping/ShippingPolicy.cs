namespace Shipping
{
    using Billing.Contracts;
    using Messages;
    using NServiceBus.Saga;
    using Sales.Contracts;

    public class ShippingPolicy : Saga<ShippingPolicyData>,
        IAmStartedByMessages<IOrderPlaced>,
        IAmStartedByMessages<IOrderBilled>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
        {
            mapper.ConfigureMapping<IOrderPlaced>(evt => evt.OrderId).ToSaga(saga => saga.OrderId);
            mapper.ConfigureMapping<IOrderBilled>(evt => evt.OrderId).ToSaga(saga => saga.OrderId);
        }


        public void Handle(IOrderPlaced message)
        {
            if (string.IsNullOrEmpty(Data.OrderId))
            {
                Data.OrderId = message.OrderId;
            }
            Data.IsOrderPlaced = true;

            if (CanShip())
            {
                Bus.Send(new ShipOrder(message.OrderId));
            }
        }

        public void Handle(IOrderBilled message)
        {
            if (string.IsNullOrEmpty(Data.OrderId))
            {
                Data.OrderId = message.OrderId;
            }
            Data.IsOrderBilled = true;

            if (CanShip())
            {
                Bus.SendLocal(new ShipOrder(message.OrderId));
            }
        }

        private bool CanShip()
        {
            return Data.IsOrderBilled && Data.IsOrderPlaced;
        }
    }

    public class ShippingPolicyData : ContainSagaData
    {
        [Unique]
        public virtual string OrderId { get; set; }
        public virtual bool IsOrderPlaced { get; set; }
        public virtual bool IsOrderBilled { get; set; }

    }
}
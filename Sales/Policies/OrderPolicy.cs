using System;

namespace Sales.Policies
{
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;

    class OrderPolicy : Saga<OrderPolicyState>, 
        IAmStartedByMessages<StartOrder>,
        IHandleMessages<PlaceOrder>,
        IHandleMessages<CancelOrder>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderPolicyState> mapper)
        {
            mapper.ConfigureMapping<StartOrder>(message => message.OrderId).ToSaga(data => data.OrderId);
            mapper.ConfigureMapping<PlaceOrder>(message => message.OrderId).ToSaga(data => data.OrderId);
            mapper.ConfigureMapping<CancelOrder>(message => message.OrderId).ToSaga(data => data.OrderId);
        }

        public void Handle(StartOrder message)
        {
            Data.OrderId = message.OrderId;
            Data.Status = OrderStatus.Started;
            //SendTimeout
        }

        public void Handle(PlaceOrder message)
        {
            Data.Status = OrderStatus.Placed;
        }

        public void Handle(CancelOrder message)
        {
            Data.Status = OrderStatus.Canceled;
        }
    }

    public class OrderPolicyState : ContainSagaData
    {
        [Unique]
        public virtual string OrderId { get; set; }
        public virtual OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Started,
        Placed,
        Canceled,
        Abandoned
    }
}

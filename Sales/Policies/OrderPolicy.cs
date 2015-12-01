using System;

namespace Sales.Policies
{
    using Contracts;
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;

    class OrderPolicy : Saga<OrderPolicyState>,
        IAmStartedByMessages<StartOrder>,
        IAmStartedByMessages<PlaceOrder>,
        IAmStartedByMessages<CancelOrder>,
        IHandleTimeouts<AbandonOrderTimeout>
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
            Data.State = OrderState.Started;
            Bus.Publish<IOrderStarted>(msg => msg.OrderId = Data.OrderId);
            RequestTimeout<AbandonOrderTimeout>(TimeSpan.FromSeconds(20));
        }

        public void Handle(PlaceOrder message)
        {
            Data.OrderId = message.OrderId;
            Data.State = OrderState.Placed;
            Bus.Publish<IOrderPlaced>(msg => msg.OrderId = Data.OrderId);
        }

        public void Handle(CancelOrder message)
        {
            Data.OrderId = message.OrderId;
            Data.State = OrderState.Canceled;
            Bus.Publish<IOrderCanceled>(msg => msg.OrderId = Data.OrderId);
        }

        public void Timeout(AbandonOrderTimeout state)
        {
            if (Data.State != OrderState.Started)
            {
                return;
            }
            Data.State = OrderState.Abandoned;
            Bus.Publish<IOrderAbandoned>(msg => msg.OrderId = Data.OrderId);
        }
    }

    public class OrderPolicyState : ContainSagaData
    {
        [Unique]
        public virtual string OrderId { get; set; }
        public virtual OrderState State { get; set; }
    }

    public enum OrderState
    {
        Unstarted,
        Started,
        Placed,
        Canceled,
        Abandoned
    }

    class AbandonOrderTimeout
    {

    }
}

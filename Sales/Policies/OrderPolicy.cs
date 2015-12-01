﻿using System;

namespace Sales.Policies
{
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;

    class OrderPolicy : Saga<OrderPolicyState>,
        IAmStartedByMessages<StartOrder>,
        IHandleMessages<PlaceOrder>,
        IHandleMessages<CancelOrder>,
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
            Data.Status = OrderStatus.Started;
            Bus.Publish<IOrderStarted>(msg => msg.OrderId = Data.OrderId);
            RequestTimeout<AbandonOrderTimeout>(TimeSpan.FromSeconds(20));
        }

        public void Handle(PlaceOrder message)
        {
            Data.Status = OrderStatus.Placed;
            Bus.Publish<IOrderPlaced>(msg => msg.OrderId = Data.OrderId);
        }

        public void Handle(CancelOrder message)
        {
            Data.Status = OrderStatus.Canceled;
            Bus.Publish<IOrderCanceled>(msg => msg.OrderId = Data.OrderId);
        }

        public void Timeout(AbandonOrderTimeout state)
        {
            if (Data.Status != OrderStatus.Started)
            {
                return;
            }
            Data.Status = OrderStatus.Abandoned;
            Bus.Publish<IOrderAbandoned>(msg => msg.OrderId = Data.OrderId);
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

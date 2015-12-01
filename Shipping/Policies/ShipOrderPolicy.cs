namespace Shipping.Handlers
{
    using System;
    using global::FedEx.Gateway.Messages;
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;
    using Ups.Gateway.Messages;

    public class ShipOrderPolicy : Saga<ShipOrderData>,
        IAmStartedByMessages<ShipOrder>,
        IHandleTimeouts<FedExTimeout>,
        IHandleMessages<FedExShipResponse>,
        IHandleMessages<UpsShipResponse>

    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShipOrderData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(msg => msg.OrderId).ToSaga(state => state.OrderId);
        }

        public void Handle(ShipOrder message)
        {
            if (string.IsNullOrEmpty(Data.OrderId))
            {
                Data.OrderId = message.OrderId;
            }

            Console.WriteLine($"Shipping order: {message.OrderId}!");

            Bus.Send(new ShipFedEx
            {
                OrderId = Data.OrderId
            });

            RequestTimeout<FedExTimeout>(TimeSpan.FromSeconds(FedEx.TimeoutInSeconds));
        }

        public void Timeout(FedExTimeout state)
        {
            if (Data.HasShipped) return;

            Bus.Send(new ShipUps { OrderId = Data.OrderId });
        }

        public void Handle(FedExShipResponse message)
        {
            Data.HasShipped = message.HasShipped;
            ReplyToOriginator(new ShipOrderResponse { HasShipped = message.HasShipped });
        }

        public void Handle(UpsShipResponse message)
        {
            Data.HasShipped = message.HasShipped;
            ReplyToOriginator(new ShipOrderResponse { HasShipped = message.HasShipped });
        }
    }

    public class FedExTimeout
    {

    }

    public class ShipOrderData : ContainSagaData
    {
        [Unique]
        public virtual string OrderId { get; set; }
        public virtual bool HasShipped { get; set; }
    }
}
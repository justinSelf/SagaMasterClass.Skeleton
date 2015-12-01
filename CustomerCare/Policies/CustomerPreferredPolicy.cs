namespace CustomerCare.Policies
{
    using System;
    using Contracts;
    using Messages;
    using NServiceBus.Saga;
    using Sales.Contracts;

    public class CustomerPreferredPolicy : Saga<CustomerPreferredData>,
        IAmStartedByMessages<IOrderPlaced>,
        IHandleTimeouts<PreferredCustomerTimeout>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CustomerPreferredData> mapper)
        {
            mapper.ConfigureMapping<IOrderPlaced>(msg => msg.CustomerId).ToSaga(saga => saga.CustomerId);
            mapper.ConfigureMapping<PreferredCustomerTimeout>(msg => msg.CustomerId).ToSaga(saga => saga.CustomerId);
        }

        public void Handle(IOrderPlaced message)
        {
            if (string.IsNullOrEmpty(Data.CustomerId))
            {
                Data.CustomerId = message.CustomerId;
            }

            Data.RunningTotal += message.OrderValue;

            if (IsCustomerPreferred())
            {
                Bus.Publish<ICustomerMadePreferred>(msg => msg.CustomerId = Data.CustomerId);
                Console.WriteLine($"Customer {message.CustomerId} now has a discount.");
            }

            var timeoutMessage = new PreferredCustomerTimeout
            {
                CustomerId = message.CustomerId,
                OrderAmount = message.OrderValue
            };
            var timeOutTimeSpan = TimeSpan.FromSeconds(20);

            RequestTimeout(timeOutTimeSpan, timeoutMessage);
        }

        bool IsCustomerPreferred()
        {
            return Data.RunningTotal >= 5000;
        }

        public void Timeout(PreferredCustomerTimeout state)
        {
            Data.RunningTotal -= state.OrderAmount;

            if (!IsCustomerPreferred())
            {
                Bus.Publish<ICustomerDemoted>();
                Console.WriteLine($"Customer {state.CustomerId} no longer has a discount.");
            }
        }
    }



    public class CustomerPreferredData : ContainSagaData
    {
        [Unique]
        public virtual string CustomerId { get; set; }
        public virtual double RunningTotal { get; set; }
    }

}
﻿using Paymaster.Payments.Helpers.Models;
using Paymaster.Payments.Logic.Interfaces;
using Paymaster.Payments.Logic.Repository;

namespace Paymaster.Payments.Logic
{
    public class PaymentsLogic : IPaymentsLogic
    {
        private readonly IPaymentsRepository paymentsRepository;

        public PaymentsLogic(IPaymentsRepository paymentsRepository)
        {
            this.paymentsRepository = paymentsRepository;
        }

        public PaymentsLogic(Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            this.paymentsRepository = new PaymentsRepository(configuration);
        }

        public int MakePayment(string requestMessage)
        {
            var paymentRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<PaymentRequest>(requestMessage);
            var t = paymentsRepository.GetAct();

            var MakeActPaymentFromPMToBalance = paymentsRepository.MakeActPaymentFromPMToBalance(paymentRequest);

            return MakeActPaymentFromPMToBalance;
        }
    }
}
using Paymaster.Payments.Domain.Models;
using Paymaster.Payments.Logic.Interfaces;
using Paymaster.Payments.Logic.Repository;
using System.Text.Json;

namespace Paymaster.Payments.Logic
{
    public class PaymentsLogic : IPaymentsLogic
    {
        private readonly IPaymentsRepository paymentsRepository;

        public PaymentsLogic(IPaymentsRepository paymentsRepository)
        {
            this.paymentsRepository = paymentsRepository;
        }

        public int MakePayment(string requestMessage)
        {
            var paymentRequest = JsonSerializer.Deserialize<PaymentRequest>(requestMessage);
            //        "@PaymentTypeId":1, 
            //        "@Summa":[TotalSum], 
            //        "@Checked":false, в конфиг вынести в каком режиме создавать пв - true / false) 
            //        "@Purpose":[notes], 
            //        "@PaymentDate": дате время текущая, 
            //        "@Inn":null, -inn предприятия из enterprises 
            //        "@Currency":"TND", 
            //        "@ExtDocId":"30000" + [ACTS2].ID, 
            //        "@LimitCover":true,

            if (paymentRequest == null)
                throw new Exception($"PaymentRequest is null");

            var actPayment = new ActPayment()
            {
                PaymentTypeId = paymentRequest.PaymentTypeId,
                Summa = paymentRequest.Summa,
                CheckedByAccounter = paymentRequest.CheckedByAccounter, // TODO чья зона ответственности?: ActBuilderServicePaymaster или Paymaster.Payments?
                Purpose = paymentRequest.Purpose,
                PaymentDate = paymentRequest.PaymentDate,
                Inn = paymentRequest.Inn,
                Currency = paymentRequest.Currency,
                ExtDocId = paymentRequest.ExtDocId,
                LimitCover = true, // TODO чья зона ответственности?: ActBuilderServicePaymaster или Paymaster.Payments?
            };

            var MakeActPaymentFromPMToBalance = paymentsRepository.MakeActPaymentFromPMToBalance(actPayment);

            return MakeActPaymentFromPMToBalance;
        }
    }
}

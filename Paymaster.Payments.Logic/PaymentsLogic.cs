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

        public int MakePayment()
        {
            var t = paymentsRepository.GetAct();

            var MakeActPaymentFromPMToBalance = paymentsRepository.MakeActPaymentFromPMToBalance(DateTime.Now);

            return MakeActPaymentFromPMToBalance;
        }
    }
}

using Paymaster.Payments.Data.Payments;

namespace Paymaster.Payments.Logic.Interfaces
{
    public interface IPaymentsRepository
    {
        public Acts2? GetAct();

        public int MakeActPaymentFromPMToBalance(DateTime dateCurrencyRate);
    }
}

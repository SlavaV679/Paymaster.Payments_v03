using Paymaster.Payments.Data.Payments;
using Paymaster.Payments.Domain.Models;

namespace Paymaster.Payments.Logic.Interfaces
{
    public interface IPaymentsRepository
    {
        public Acts2? GetAct();

        public int MakeActPaymentFromPMToBalance(ActPayment actPayment);
    }
}

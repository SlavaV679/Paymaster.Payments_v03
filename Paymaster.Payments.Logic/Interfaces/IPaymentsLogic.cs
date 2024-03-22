namespace Paymaster.Payments.Logic.Interfaces
{
    public interface IPaymentsLogic
    {
        public int MakePayment(string requestMessage);
    }
}

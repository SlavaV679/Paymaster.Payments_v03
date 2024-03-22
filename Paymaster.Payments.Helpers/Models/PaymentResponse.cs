namespace Paymaster.Payments.Helpers.Models
{
    public class PaymentResponse
    {
        public string Message { get; set; }

        public string ErrorCode { get; set; }

        public static PaymentResponse Deserialize(string reqMsg)
        {
            throw new NotImplementedException();
        }
    }
}

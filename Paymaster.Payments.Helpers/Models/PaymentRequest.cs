namespace Paymaster.Payments.Helpers.Models
{
    public class PaymentRequest
    {
        public string ActId { get; set; }

        public long ExtDocId { get; set; }

        public string PaymentTypeId { get; set; }

        public DateTime PaymentDate { get; set; }

        public long Summa { get; set; }

        public string Currency { get; set; }

        public string Purpose { get; set; }

        public static object Deserialize(string requestRaw)
        {
            throw new NotImplementedException();
        }

        public char[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}

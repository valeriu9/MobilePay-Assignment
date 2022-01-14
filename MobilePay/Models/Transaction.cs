namespace MobilePay.Models
{
    using System;

    public class Transaction
    {
        public Guid ID { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal Amount { get; set; }

        public string MerchantName { get; set; }
    }
}

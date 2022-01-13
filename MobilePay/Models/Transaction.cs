using System;

namespace MobilePay.Models
{
    public class Transaction
    {
        public int ID { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal Amount { get; set; }

        public string MerchantName { get; set; }
    }
}

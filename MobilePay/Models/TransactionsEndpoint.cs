using System.Collections.Generic;

namespace MobilePay.Models
{
    public class TransactionsEndpoint
    {
        public Transaction Transaction { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}

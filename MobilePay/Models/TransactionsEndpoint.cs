namespace MobilePay.Models
{
    using System.Collections.Generic;

    public class TransactionsEndpoint
    {
        public Transaction Transaction { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobilePay.Models
{
    public class TransactionsEndpoint
    {
        public Transaction Transaction { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}

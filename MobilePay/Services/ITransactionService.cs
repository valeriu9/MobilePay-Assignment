using MobilePay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobilePay.Services
{
    public interface ITransactionService
    {
        void RergisterTransaction(Transaction transaction);
        void RegisterTransactions(List<Transaction> transactions);
        List<Transaction> GetMerchantTransactions(string merchantName);
        decimal GetFee(List<Transaction> transactions);
    }
}

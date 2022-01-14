namespace MobilePay.Services
{
    using MobilePay.Models;
    using System.Collections.Generic;

    public interface ITransactionService
    {
        void RergisterTransaction(Transaction transaction);

        void RegisterTransactions(List<Transaction> transactions);

        List<Transaction> GetMerchantTransactions(string merchantName);

        decimal GetFee(List<Transaction> transactions);
    }
}

using MobilePay.Calculations;
using MobilePay.Models;
using MobilePay.QueueRequests;
using MobilePay.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobilePay.Services
{
    public class TransactionService : ITransactionService
    {
        public decimal GetFee(List<Transaction> transactions)
        {
            var feeCalculator = new FeeCalculator();
            
            return feeCalculator.CalculateFee(transactions);
        }


        public List<Transaction> GetMerchantTransactions(string merchantName)
        {
            return getMerchantTransactions(merchantName);
        }

        public void RegisterTransactions(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                QueueJobs.Instance.queue.Enqueue(transaction);
            }
            QueueJobs.DequeueAndSave();
        }

        public void RergisterTransaction(Transaction transaction)
        {
            QueueJobs.Instance.queue.Enqueue(transaction);
            QueueJobs.DequeueAndSave();
        }

        private List<Transaction> getMerchantTransactions(string merchantName)
        {
            var dataWritter = new ReadWrtieData();
            var data = dataWritter.ReadDataFromFile();

            return data.FindAll(transaction => transaction.MerchantName == merchantName);
        }
    }
}

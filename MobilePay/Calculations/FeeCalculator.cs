namespace MobilePay.Calculations
{
    using MobilePay.Models;
    using MobilePay.Storage;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FeeCalculator
    {
        public decimal CalculateFee(List<Transaction> transactions)
        {
            if (transactions == null || transactions.Count == 0)
            {
                return 0;
            }
            var discountList = new ReadWriteData().ReadDiscountsFromFile();
            decimal discountPercentage = 0;
            decimal discountOfMerchant = 0;
            decimal initialFee = 0;
            var hasAdditionalDiscount = checkTransactionsPeriod(transactions);
            decimal additionalDiscount = 0;

            foreach (var transaction in transactions)
            {
                initialFee += getTransactionFee(transaction);
            }

            if (discountList.SingleOrDefault(discount => discount.MerchantName == transactions[0].MerchantName) != null)
            {
                discountPercentage = discountList.SingleOrDefault(discount => discount.MerchantName == transactions[0].MerchantName).DiscountPercentage;
                discountOfMerchant = initialFee * (discountPercentage / 100);
            }
            additionalDiscount = hasAdditionalDiscount ? (initialFee - discountOfMerchant) * (decimal)0.2 : 0;
            var totalFee = initialFee - discountOfMerchant - additionalDiscount;
            return totalFee;
        }

        private decimal getTransactionFee(Transaction transaction)
        {

            var result = (transaction.Timestamp.DayOfWeek == DayOfWeek.Saturday) || (transaction.Timestamp.DayOfWeek == DayOfWeek.Sunday) ? 0 : transaction.Amount * (decimal)0.01;
            return result;
        }

        private bool checkTransactionsPeriod(List<Transaction> transactions)
        {
            var monthsInList = new List<MonthCounter>();
            foreach (var transaction in transactions)
            {
                if (!monthsInList.Exists(element => element.Year == transaction.Timestamp.Year && element.Month == transaction.Timestamp.Month))
                {
                    monthsInList.Add(new MonthCounter() { Year = transaction.Timestamp.Year, Month = transaction.Timestamp.Month, Counter = 0 });
                }
                else
                {
                    monthsInList[monthsInList.FindIndex(element => element.Year == transaction.Timestamp.Year && element.Month == transaction.Timestamp.Month)].Counter += 1;
                }
            }
            return monthsInList.Exists(element => element.Counter + 1 >= 10);
        }
    }
}

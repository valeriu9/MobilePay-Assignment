using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobilePay.Calculations;
using MobilePay.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePay.Calculations.Tests
{
    [TestClass]
    public class FeeCalculatorTests
    {
        // Merchant without discount
        [TestMethod]
        public void CalculateFee_TransactionsInWeekDay_ReturnsOnePercentFee()
        {
            var transactions = new List<Transaction> { 
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Mercedes", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:31:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Mercedes", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:41:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Mercedes", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:51:14.056Z") }
                    };
            var feeCalculator = new FeeCalculator();

            var actual = feeCalculator.CalculateFee(transactions);

            Assert.AreEqual(10500, actual);
        }

        [TestMethod]
        public void CalculateFee_TransactionsInWeekendDay_ReturnsZeroFee()
        {
            var transactions = new List<Transaction> {
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Mercedes", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:31:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Mercedes", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:41:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Mercedes", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:51:14.056Z") }
                    };
            var feeCalculator = new FeeCalculator();

            var actual = feeCalculator.CalculateFee(transactions);

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void CalculateFee_TransactionsInWeekendAndWeekDays_ReturnsFeeForTransactionsInWeekDays()
        {
            var transactions = new List<Transaction> {
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Mercedes", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:31:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Mercedes", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:41:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Mercedes", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:51:14.056Z") }
                    };
            var feeCalculator = new FeeCalculator();

            var actual = feeCalculator.CalculateFee(transactions);

            Assert.AreEqual(3500, actual);
        }

        // Merchant with discount

        [TestMethod]
        public void CalculateFee_TransactionsInWeekDayWithDiscount_ReturnsOnePercentFee()
        {
            var transactions = new List<Transaction> {
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:31:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:41:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:51:14.056Z") }
                    };
            var feeCalculator = new FeeCalculator();

            var actual = feeCalculator.CalculateFee(transactions);

            Assert.AreEqual(7875, actual);
        }

        [TestMethod]
        public void CalculateFee_TransactionsInWeekendDayWithDiscount_ReturnsZeroFee()
        {
            var transactions = new List<Transaction> {
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:31:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:41:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:51:14.056Z") }
                    };
            var feeCalculator = new FeeCalculator();

            var actual = feeCalculator.CalculateFee(transactions);

            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void CalculateFee_TransactionsInWeekendAndWeekDaysWithDiscount_ReturnsFeeForTransactionsInWeekDays()
        {
            var transactions = new List<Transaction> {
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:31:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:41:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-25T13:51:14.056Z") }
                    };
            var feeCalculator = new FeeCalculator();

            var actual = feeCalculator.CalculateFee(transactions);

            Assert.AreEqual(2625, actual);
        }
        // Merchant with extra discount because he has more than 10 transactions in a month
        [TestMethod]
        public void CalculateFee_MoreThanTenTransactionInMonth_ReturnsExtraTwentyPercentDicount()
        {
            var transactions = new List<Transaction> {
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:01:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:11:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:21:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:31:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:41:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:51:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T13:55:10.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T14:01:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T14:11:14.056Z") },
                    new Transaction { ID = Guid.NewGuid(), MerchantName = "Tesla", Amount = 350000, Timestamp = DateTime.Parse("2021-12-24T14:21:14.056Z") }
                    };
            var feeCalculator = new FeeCalculator();

            var actual = feeCalculator.CalculateFee(transactions);

            Assert.AreEqual(21000, actual);
        }

        [TestMethod]
        public void CalculateFee_EmptyListOfTransactions_ReturnsZero()
        {
            var transactions = new List<Transaction> {};
            var feeCalculator = new FeeCalculator();

            var actual = feeCalculator.CalculateFee(transactions);

            Assert.AreEqual(0, actual);
        }
    }
}
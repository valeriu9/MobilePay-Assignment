using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobilePay.Calculations;
using MobilePay.Models;
using MobilePay.QueueRequests;
using MobilePay.Storage;
using System;
using System.Collections.Generic;

namespace MobilePay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {

        readonly ILogger<TransactionController> _logger;

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("~/register-transactions")]
        public IActionResult RegisterTransactions([FromBody] TransactionsEndpoint transactionEndpoint)
        {
            _logger.LogInformation("Registering transactions: {@transactionEndpoint} ", transactionEndpoint);

            if (transactionEndpoint.Transactions != null) {
                _logger.LogInformation("Add to queue the transactions list");
                foreach (var transaction in transactionEndpoint.Transactions)
                {
                QueueJobs.Instance.queue.Enqueue(transaction);
                }
            } 
            if(transactionEndpoint.Transaction != null)
            {
                _logger.LogInformation("Add to queue the transaction");
                QueueJobs.Instance.queue.Enqueue(transactionEndpoint.Transaction);
            }
            if(transactionEndpoint.Transaction != null && transactionEndpoint.Transactions != null)
            {
                _logger.LogWarning("Register transaction failed due to missing data {@transactionEndpoint}", transactionEndpoint);
                return StatusCode(400, "Insert a transaction or a list of transactions");
            }
            _logger.LogInformation("Dequeue and save to file performed");
            QueueJobs.DequeueAndSave();

            return Ok();
        }

        [HttpGet("~/get-transactions")]
        public IActionResult GetTransactionsByMerchant([FromQuery] string merchantName)
        {
            _logger.LogInformation("Requesting transactions for: {merchantName}", merchantName);
            return Ok(getMerchantTransactions(merchantName));
        }


        [HttpGet("~/get-fee")]
        public IActionResult GetFeeByMerchant([FromQuery] string merchantName)
        {
            _logger.LogInformation("Requesting fee for: {merchantName}", merchantName);
            var feeCalculator = new FeeCalculator();
            var merchantTransactions = getMerchantTransactions(merchantName);
            if(merchantTransactions.Count == 0)
            {
                _logger.LogWarning("{merchant} does not have a transaction", merchantName);
                return StatusCode(404, "This merchant does not have a transaction");
            } 
            return Ok(feeCalculator.CalculateFee(merchantTransactions));
        }

        [NonAction]
        private List<Transaction> getMerchantTransactions(string merchantName)
        {
            var dataWritter = new ReadWrtieData();
            var data = dataWritter.ReadDataFromFile();

            return data.FindAll(transaction => transaction.MerchantName == merchantName);
        }
    }
}

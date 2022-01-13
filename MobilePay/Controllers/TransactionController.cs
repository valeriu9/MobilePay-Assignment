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

        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
        }

        [HttpPost("~/register-transactions")]
        public IActionResult RegisterTransactions([FromBody] TransactionsEndpoint transactionEndpoint)
        {
            if(transactionEndpoint.Transactions != null) { 
                foreach(var transaction in transactionEndpoint.Transactions)
                {
                QueueJobs.Instance.queue.Enqueue(transaction);
                }
            } 
            if(transactionEndpoint.Transaction != null)
            {
            QueueJobs.Instance.queue.Enqueue(transactionEndpoint.Transaction);
            }
            if(transactionEndpoint.Transaction != null && transactionEndpoint.Transactions != null)
            {
                return StatusCode(400, "Insert a transaction or a list of transactions");
            }

            QueueJobs.DequeueAndSave();

            return Ok();
        }

        [HttpGet("~/get-transactions")]
        public IActionResult GetTransactionsByMerchant([FromQuery] string merchantName)
        {
            return Ok(getMerchantTransactions(merchantName));
        }
        [HttpGet("~/get-fee")]
        public IActionResult GetFeeByMerchant([FromQuery] string merchantName)
        {
            var feeCalculator = new FeeCalculator();
            var merchantTransactions = getMerchantTransactions(merchantName);
            if(merchantTransactions.Count == 0)
            {
                return Ok("This merchant does not have a transaction");
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

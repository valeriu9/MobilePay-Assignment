using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobilePay.Calculations;
using MobilePay.Models;
using MobilePay.QueueRequests;
using MobilePay.Services;
using MobilePay.Storage;
using System;
using System.Collections.Generic;

namespace MobilePay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        readonly ITransactionService _transaction;
        readonly ILogger<TransactionController> _logger;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transaction)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _transaction = transaction;
        }


        [HttpPost("~/register-transactions")]
        public IActionResult RegisterTransactions([FromBody] TransactionsEndpoint transactionEndpoint)
        {
            _logger.LogInformation("Registering transactions: {@transactionEndpoint} ", transactionEndpoint);

            if(transactionEndpoint.Transaction != null && transactionEndpoint.Transactions != null)
            {
                _logger.LogWarning("Register transaction failed due to missing data {@transactionEndpoint}", transactionEndpoint);
                return StatusCode(400, "Insert a transaction or a list of transactions");
            }
            if (transactionEndpoint.Transactions != null) {
                _logger.LogInformation("Add to queue the transactions list");
                _transaction.RegisterTransactions(transactionEndpoint.Transactions);
            } 
            if(transactionEndpoint.Transaction != null)
            {
                _logger.LogInformation("Add to queue the transaction");
                _transaction.RergisterTransaction(transactionEndpoint.Transaction);
            }
            _logger.LogInformation("Dequeue and save to file performed");
            
            return Ok("Successfully added!");
        }


        [HttpGet("~/get-transactions")]
        public IActionResult GetTransactionsByMerchant([FromQuery] string merchantName)
        {
            _logger.LogInformation("Requesting transactions for: {merchantName}", merchantName);
            var result = _transaction.GetMerchantTransactions(merchantName);
            if(result.Count == 0)
            {
                _logger.LogInformation("Merchant: {merchantName} does not have transactions", merchantName);
                return StatusCode(200, "The merchant does not have transactions");
            }
            return Ok(result);
        }


        [HttpGet("~/get-fee")]
        public IActionResult GetFeeByMerchant([FromQuery] string merchantName)
        {
            _logger.LogInformation("Requesting fee for: {merchantName}", merchantName);
            var transactionsForMerchant = _transaction.GetMerchantTransactions(merchantName);
            if (transactionsForMerchant.Count == 0)
            {
                _logger.LogWarning("{merchant} does not have a transaction", merchantName);
                return StatusCode(200, "The merchant does not have a transaction");
            }
            var result = _transaction.GetFee(transactionsForMerchant);
            return Ok(result);
        }       
    }
}

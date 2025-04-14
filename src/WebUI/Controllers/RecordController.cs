using CleanArchitecture.Application.Transactions.Commands;
using CleanArchitecture.Application.Transactions.Commands.DeleteTransaction;
using CleanArchitecture.Application.Transactions.Commands.UpdateTransaction;
using CleanArchitecture.Application.Transactions.Queries;
using CleanArchitecture.Application.Transactions.Queries.GetTransactionById;
using CleanArchitecture.Application.Transactions.Queries.GetTransactionReport;
using CleanArchitecture.Application.Transactions.Queries.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using CleanArchitecture.Application.Transactions._Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.Wallets.Queries.GetAllWallet;
using CleanArchitecture.Application.Transactions.Queries.GetTransactionAll;
using CleanArchitecture.Application.Wallets._Dto;


namespace CleanArchitecture.WebUI.Controllers
{
    public class RecordController : ApiController
    {
        [HttpGet("report")]
        public async Task<ActionResult<List<TransactionReportDto>>> GetTransactionReport()
        {
            var result = await Mediator.Send(new GetTransactionReport());
            return Ok(result);
        }

        [HttpGet("walletbalance")]
        public async Task<ActionResult<List<WalletUserDto>>> GetWallets([FromQuery] GetAllWalletQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("AllTransaction")]
        public async Task<ActionResult<List<TransactionReportDto>>> GetTransactionAllReport()
        {
            var result = await Mediator.Send(new GetTransactionAllQuery());
            return Ok(result);
        }
    }
}

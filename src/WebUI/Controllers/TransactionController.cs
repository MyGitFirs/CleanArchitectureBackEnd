using CleanArchitecture.Application.Transactions.Commands;
using CleanArchitecture.Application.Transactions.Commands.DeleteTransaction;
using CleanArchitecture.Application.Transactions.Commands.UpdateTransaction;
using CleanArchitecture.Application.Transactions.Queries;
using CleanArchitecture.Application.Transactions.Queries.GetTransactionById;
using CleanArchitecture.Application.Transactions.Queries.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.WebUI.Controllers
{
    public class TransactionController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] GetTransactionQuery request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            try
            {
                var query = new GetTransactionByIdQuery { Id = id };
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetTransactionByUserID(Guid id)
        {
            try
            {
                var query = new GetTransactionByUserId { UserId = id };
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] CreateTransactionCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(string id, [FromBody] UpdateTransactionCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            if (!Guid.TryParse(id, out Guid transactionId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var request = new DeleteTransactionCommand
            {
                Id = transactionId
            };

            var result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
using CleanArchitecture.Application.Wallets.Commands;
using CleanArchitecture.Application.Wallets.Commands.DeleteWallet;
using CleanArchitecture.Application.Wallets.Commands.UpdateWallet;
using CleanArchitecture.Application.Wallets.Queries;
using CleanArchitecture.Application.Wallets.Queries.GetWallets;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.WebUI.Controllers
{
    public class WalletController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetWallets([FromQuery] GetWalletQuery request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWallet(Guid id)
        {
            try
            {
                var query = new GetWalletByIdQuery { Id = id };
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWallet([FromBody] CreateWalletCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(string id, [FromBody] UpdateWalletCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(string id)
        {
            if (!Guid.TryParse(id, out Guid walletId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var request = new DeleteWalletCommand
            {
                Id = walletId
            };

            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("create-company-wallet")]
        public async Task<IActionResult> CreateCompanyWallet([FromBody] CreateWalletForCompanyCommand request)
        {
            var result = await Mediator.Send(request);
            if (result == 0)
                return BadRequest("No users found without a wallet in this company.");

            return Ok(new { message = $"{result} wallets created for company users." });
        }

        [HttpPost("add-bonus")]
        public async Task<IActionResult> AddBonus([FromBody] CreateBonusWallet request)
        {
            try
            {
                var result = await Mediator.Send(request);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}


using CleanArchitecture.Application.Companies.Commands.CreateCompany;
using CleanArchitecture.Application.Companies.Commands.DeleteCompany;
using CleanArchitecture.Application.Companies.Commands.JoinCompany;
using CleanArchitecture.Application.Company.Commands.UpdateCompany;
using CleanArchitecture.Application.Company.Queries.GetCompanies;
using CleanArchitecture.Application.User.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CleanArchitecture.WebUI.Controllers
{
    public class CompanyController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetCompanies([FromQuery] GetCompanyQuery request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            try
            {
                var query = new GetCompanyByIdQuery { Id = id };
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] CreateCompanyCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(string id, [FromBody] UpdateCompanyCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
        [HttpPost("join-company")]
        public async Task<IActionResult> JoinCompany([FromBody] JoinUserCompanyCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            if (!Guid.TryParse(id, out Guid companyId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var request = new DeleteCompanyCommand
            {
                Id = companyId
            };

            var result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}

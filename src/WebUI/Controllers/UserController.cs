using CleanArchitecture.Application.User.Commands;
using CleanArchitecture.Application.User.Commands.UpdateUser;
using CleanArchitecture.Application.User.Queries.GetUsers;
using CleanArchitecture.Application.Users.Commands.DeleteUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.WebUI.Controllers
{
    public class UserController: ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] GetUserQuery request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var query = new GetUserByIdQuery { Id = id };
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetUserByCompanyId(Guid id)
        {
            try
            {
                var query = new GetUsersByCompanyIdQuery { CompanyId = id };
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var request = new DeleteUserCommand
            {
                Id = userId
            };

            var result = await Mediator.Send(request);
            return Ok(result);
        }
        

    }
}

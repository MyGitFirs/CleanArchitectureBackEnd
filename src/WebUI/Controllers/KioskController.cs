using CleanArchitecture.Application.Kiosks.Commands;
using CleanArchitecture.Application.Kiosks.Commands.DeleteKiosk;
using CleanArchitecture.Application.Kiosks.Commands.UpdateKiosk;
using CleanArchitecture.Application.Kiosks.Queries.GetKiosks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.WebUI.Controllers
{
    public class KioskController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetKiosks([FromQuery] GetKioskQuery request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKiosk(Guid id)
        {
            try
            {
                var query = new GetKioskByIdQuery { Id = id };
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddKiosk([FromBody] CreateKioskCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKiosk(string id, [FromBody] UpdateKioskCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKiosk(string id)
        {
            if (!Guid.TryParse(id, out Guid kioskId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var request = new DeleteKioskCommand
            {
                Id = kioskId
            };

            var result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
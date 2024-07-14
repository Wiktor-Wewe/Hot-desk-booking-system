using Hdbs.Core.DTOs;
using Hdbs.Transfer.Desks.Commands;
using Hdbs.Transfer.Desks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hot_desk_booking_system.Controllers
{
    [Route("api/Desk")]
    [ApiController]
    //[Authorize]
    public class DeskController : Controller
    {
        private readonly IMediator _mediator;

        public DeskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize(Policy = "SimpleView")]
        [HttpGet]
        public async Task<IActionResult> ListDesksAsync([FromQuery] ListDesksQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeskAsync([FromRoute] Guid id)
        {
            var query = new GetDeskQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeskAsync([FromBody] CreateDeskCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPost("{id}/Reserve")]
        public async Task<IActionResult> ReserveDeskAsync([FromRoute] Guid id, [FromBody] ReserveDeskCommand command)
        {
            command.DeskId = id;
            command.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _mediator.Send(command);
            return Ok(response.ToResponseDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeskAsync([FromRoute] Guid id, [FromBody] UpdateDeskCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeskAsync([FromRoute] Guid id)
        {
            var command = new DeleteDeskCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}

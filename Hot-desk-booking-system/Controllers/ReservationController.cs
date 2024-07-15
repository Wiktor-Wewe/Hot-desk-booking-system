using Hdbs.Core.DTOs;
using Hdbs.Transfer.Reservations.Commands;
using Hdbs.Transfer.Reservations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hot_desk_booking_system.Controllers
{
    [Authorize]
    [Route("api/Reservation")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = "AdminView")]
        [HttpGet]
        public async Task<IActionResult> ListReservationsAsync([FromQuery] ListReservationsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "AdminView")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationAsync([FromRoute] Guid id)
        {
            var query = new GetReservationQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "SimpleView")]
        [HttpGet("MyReservations")]
        public async Task<IActionResult> ListMyReservationsAsync([FromQuery] ListMyReservationsQuery query)
        {
            query.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "CreateReservation")]
        [HttpPost]
        public async Task<IActionResult> CreateReservationAsync([FromBody] CreateReservationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "UpdateReservation")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservationAsync([FromRoute] Guid id, [FromBody] UpdateReservationCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = "SimpleView")]
        [HttpPut("{id}/Update")]
        public async Task<IActionResult> UpdateMyReservationAsync([FromRoute] Guid id, [FromBody] UpdateMyReservationCommand command)
        {
            command.ReservationId = id;
            command.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = "DeleteReservation")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservationAsync([FromRoute] Guid id)
        {
            var command = new DeleteReservationCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}

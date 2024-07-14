using Hdbs.Core.DTOs;
using Hdbs.Transfer.Reservations.Commands;
using Hdbs.Transfer.Reservations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Claims;

namespace Hot_desk_booking_system.Controllers
{
    [Route("api/Reservation")]
    public class ReservationController : Controller
    {
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListReservationsAsync([FromQuery] ListReservationsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationAsync([FromRoute] Guid id)
        {
            var query = new GetReservationQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [HttpGet("MyReservations")]
        public async Task<IActionResult> ListMyReservationsAsync([FromQuery] ListMyReservationsQuery query)
        {
            query.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservationAsync([FromBody] CreateReservationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservationAsync([FromRoute] Guid id, [FromBody] UpdateReservationCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservationAsync([FromRoute] Guid id)
        {
            var command = new DeleteReservationCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}

using Hdbs.Core.DTOs;
using Hdbs.Transfer.Locations.Commands;
using Hdbs.Transfer.Locations.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hot_desk_booking_system.Controllers
{
    [Authorize]
    [Route("api/Location")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = "SimpleView")]
        [HttpGet]
        public async Task<IActionResult> ListLocationsAsync([FromQuery] ListLocationsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "SimpleView")]
        [HttpGet("{id}/Desks")]
        public async Task<IActionResult> ListDesksAsync([FromRoute] Guid id, [FromQuery] ListDesksByLocationQuery query)
        {
            query.LocationId = id;
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "SimpleView")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationAsync([FromRoute] Guid id)
        {
            var query = new GetLocationQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "AdminView")]
        [HttpGet("{id}/Reservations")]
        public async Task<IActionResult> ListReservationsAsync([FromRoute] Guid id, [FromQuery] ListReservationsByLocationQuery query)
        {
            query.LocationId = id;
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "CreateLocation")]
        [HttpPost]
        public async Task<IActionResult> CreateLocationAsync([FromBody] CreateLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "UpdateLocation")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocationAsync([FromRoute] Guid id, [FromBody] UpdateLocationCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = "DeleteLocation")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationAsync([FromRoute] Guid id)
        {
            var command = new DeleteLocationCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}

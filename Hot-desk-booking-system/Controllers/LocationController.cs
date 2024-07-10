using Hdbs.Core.DTOs;
using Hdbs.Transfer.Locations.Commands;
using Hdbs.Transfer.Locations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hot_desk_booking_system.Controllers
{
    [Route("api/Location")]
    [ApiController]
    public class LocationController : Controller
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListLocationsAsync([FromQuery] ListLocationsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationAsync([FromRoute] Guid id)
        {
            var query = new GetLocationQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocationAsync([FromBody] CreateLocationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocationAsync([FromRoute] Guid id, [FromBody] UpdateLocationCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationAsync([FromRoute] Guid id)
        {
            var command = new DeleteLocationCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}

using Hdbs.Core.DTOs;
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
    }
}

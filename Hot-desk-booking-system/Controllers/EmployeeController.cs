using Hdbs.Core.DTOs;
using Hdbs.Transfer.Employees.Commands;
using Hdbs.Transfer.Employees.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hot_desk_booking_system.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> ListEmployeeAsync([FromQuery] ListEmployeesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeAsync([FromRoute] Guid id)
        {
            var query = new GetEmployeeQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.ToResponseDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] Guid id, [FromBody] UpdateEmployeeCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] Guid id)
        {
            var command = new DeleteEmployeeCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }
    }
}

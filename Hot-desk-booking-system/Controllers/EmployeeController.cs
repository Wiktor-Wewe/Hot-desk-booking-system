﻿using Hdbs.Core.DTOs;
using Hdbs.Transfer.Employees.Commands;
using Hdbs.Transfer.Employees.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Hot_desk_booking_system.Controllers
{
    [Authorize]
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginEmployeeAsync([FromBody] LoginEmployeeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "SetPermissions")]
        [HttpPost("{id}/Permissions")]
        public async Task<IActionResult> SetPermissionsAsync([FromRoute] string id, [FromBody] SetPermissionsForEmployeeCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = "AdminView")]
        [HttpGet]
        public async Task<IActionResult> ListEmployeeAsync([FromQuery] ListEmployeesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "SimpleView")]
        [HttpGet("Me")]
        public async Task<IActionResult> GetMeAsync()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetMeEmployeeQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "AdminView")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeAsync([FromRoute] string id)
        {
            var query = new GetEmployeeQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "AdminView")]
        [HttpGet("{id}/Reservations")]
        public async Task<IActionResult> ListReservationsAsync([FromRoute] string id, [FromQuery] ListReservationsByEmployeeQuery query)
        {
            query.EmployeeId = id;
            var result = await _mediator.Send(query);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "CreateEmployee")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.ToResponseDto());
        }

        [Authorize(Policy = "UpdateEmployee")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] string id, [FromBody] UpdateEmployeeCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = "SimpleView")]
        [HttpPut("UpdateMe")]
        public async Task<IActionResult> UpdateMyEmployeeAsync([FromBody] UpdateMyEmployeeCommand command)
        {
            command.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = "DeleteEmployee")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] string id)
        {
            var command = new DeleteEmployeeCommand { Id = id };
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = "SetEmployeeStatus")]
        [HttpPut("{id}/SetStatus")]
        public async Task<IActionResult> SetStatusAsync([FromRoute] string id, [FromBody] SetStatusForEmployeeCommand command)
        {
            command.EmployeeId = id;
            await _mediator.Send(command);
            return Ok();
        }

        [Authorize(Policy = "ImportEmployee")]
        [HttpGet("GetImportExcel")]
        public async Task<IActionResult> GetImportExcel()
        {
            var query = new GetImportExcelEmployeeQuery();
            var excelStream = await _mediator.Send(query);
            string fileName = $"Hdbs_employee.xlsx";
            return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [Authorize(Policy = "ImportEmployee")]
        [HttpPost("ImportExcel")]
        public async Task<IActionResult> ImportExcel([FromForm] ImportExcelEmployeeCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result.ToResponseDto());
        }
    }
}

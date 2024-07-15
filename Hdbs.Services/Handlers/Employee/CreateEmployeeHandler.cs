using Hdbs.Services.Implementations;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Desks.Commands;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Employees.Commands;
using Hdbs.Transfer.Employees.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Handlers.Employee
{
    public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<CreateEmployeeHandler> _logger;

        public CreateEmployeeHandler(IEmployeeService employeeService, ILogger<CreateEmployeeHandler> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _employeeService.CreateAsync(request);
        }
    }
}

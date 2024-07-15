using Hdbs.Services.Implementations;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Desks.Commands;
using Hdbs.Transfer.Employees.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Handlers.Employee
{
    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<UpdateEmployeeHandler> _logger;

        public UpdateEmployeeHandler(IEmployeeService employeeService, ILogger<UpdateEmployeeHandler> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _employeeService.UpdateAsync(request);
        }
    }
}

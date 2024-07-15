using Hdbs.Services.Implementations;
using Hdbs.Services.Interfaces;
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
    public class LoginEmployeeHandler : IRequestHandler<LoginEmployeeCommand, string>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<LoginEmployeeHandler> _logger;

        public LoginEmployeeHandler(IEmployeeService employeeService, ILogger<LoginEmployeeHandler> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task<string> Handle(LoginEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _employeeService.LoginEmployeeAsync(request);
        }
    }
}

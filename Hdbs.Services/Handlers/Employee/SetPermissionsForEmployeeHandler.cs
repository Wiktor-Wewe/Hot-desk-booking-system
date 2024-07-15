using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Employees.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Services.Handlers.Employee
{
    public class SetPermissionsForEmployeeHandler : IRequestHandler<SetPermissionsForEmployeeCommand>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<SetPermissionsForEmployeeHandler> _logger;

        public SetPermissionsForEmployeeHandler(IEmployeeService employeeService, ILogger<SetPermissionsForEmployeeHandler> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task Handle(SetPermissionsForEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _employeeService.SetPermissionsAsync(request);
        }
    }
}

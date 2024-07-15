using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Employees.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Services.Handlers
{
    public class SetStatusForEmployeeHandler : IRequestHandler<SetStatusForEmployeeCommand>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<SetStatusForEmployeeHandler> _logger;

        public SetStatusForEmployeeHandler(IEmployeeService employeeService, ILogger<SetStatusForEmployeeHandler> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task Handle(SetStatusForEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _employeeService.SetStatusAsync(request);
        }
    }
}

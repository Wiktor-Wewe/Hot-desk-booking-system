using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Employees.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Services.Handlers.Employee
{
    public class UpdateMyEmployeeHandler : IRequestHandler<UpdateMyEmployeeCommand>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<UpdateMyEmployeeHandler> _logger;

        public UpdateMyEmployeeHandler(IEmployeeService employeeService, ILogger<UpdateMyEmployeeHandler> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task Handle(UpdateMyEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            await _employeeService.UpdateMyAsync(request);
        }
    }
}

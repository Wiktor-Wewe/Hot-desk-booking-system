using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Employees.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Repositories.Handlers
{
    public class GetEmployeeHandler : IRequestHandler<GetEmployeeQuery, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<GetEmployeeHandler> _logger;

        public GetEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<GetEmployeeHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _employeeRepository.GetAsync(request);
        }
    }
}

using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Employees.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Repositories.Handlers.Employee
{
    public class GetMeEmployeeHandler : IRequestHandler<GetMeEmployeeQuery, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<GetMeEmployeeHandler> _logger;

        public GetMeEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<GetMeEmployeeHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<EmployeeDto> Handle(GetMeEmployeeQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _employeeRepository.GetMeEmployeeAsync(request);
        }
    }
}

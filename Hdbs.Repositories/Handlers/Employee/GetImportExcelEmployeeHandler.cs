using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Employees.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Repositories.Handlers.Employee
{
    public class GetImportExcelEmployeeHandler : IRequestHandler<GetImportExcelEmployeeQuery, MemoryStream>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<GetImportExcelEmployeeHandler> _logger;

        public GetImportExcelEmployeeHandler(IEmployeeRepository employeeRepository, ILogger<GetImportExcelEmployeeHandler> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<MemoryStream> Handle(GetImportExcelEmployeeQuery request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _employeeRepository.GetImportExcelAsync(request);
        }
    }
}

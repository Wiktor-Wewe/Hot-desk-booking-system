using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Employees.Commands;
using Hdbs.Transfer.Employees.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hdbs.Services.Handlers.Employee
{
    public class ImportExcelEmployeeHandler : IRequestHandler<ImportExcelEmployeeCommand, ImportExcelEmployeeResponse>
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ImportExcelEmployeeHandler> _logger;

        public ImportExcelEmployeeHandler(IEmployeeService employeeService, ILogger<ImportExcelEmployeeHandler> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public async Task<ImportExcelEmployeeResponse> Handle(ImportExcelEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("Operation was cancelled.");
                throw new OperationCanceledException("Operation was cancelled.");
            }

            return await _employeeService.ImportExcelEmployeeAsync(request);
        }
    }
}

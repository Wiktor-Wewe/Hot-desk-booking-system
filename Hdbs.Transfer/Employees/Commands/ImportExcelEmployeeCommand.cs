using Hdbs.Transfer.Employees.Data;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hdbs.Transfer.Employees.Commands
{
    public class ImportExcelEmployeeCommand : IRequest<ImportExcelEmployeeResponse>
    {
        public IFormFile File { get; set; } = null!;
    }
}

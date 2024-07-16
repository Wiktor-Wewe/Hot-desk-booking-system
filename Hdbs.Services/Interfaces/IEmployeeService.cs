using Hdbs.Transfer.Employees.Commands;
using Hdbs.Transfer.Employees.Data;

namespace Hdbs.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<string> LoginEmployeeAsync(LoginEmployeeCommand command);
        Task SetPermissionsAsync(SetPermissionsForEmployeeCommand command);
        Task<EmployeeDto> CreateAsync(CreateEmployeeCommand command);
        Task UpdateAsync(UpdateEmployeeCommand command);
        Task UpdateMyAsync(UpdateMyEmployeeCommand command);
        Task DeleteAsync(DeleteEmployeeCommand command);
        Task SetStatusAsync(SetStatusForEmployeeCommand command);
        Task<ImportExcelEmployeeResponse> ImportExcelEmployeeAsync(ImportExcelEmployeeCommand command);
    }
}

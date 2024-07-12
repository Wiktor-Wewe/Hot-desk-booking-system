using Hdbs.Transfer.Employees.Commands;
using Hdbs.Transfer.Employees.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<string> LoginEmployeeAsync(LoginEmployeeCommand command);
        Task SetPermissionsAsync(SetPermissionsForEmployeeCommand command);
        Task<EmployeeDto> CreateAsync(CreateEmployeeCommand command);
        Task UpdateAsync(UpdateEmployeeCommand command);
        Task DeleteAsync(DeleteEmployeeCommand command);
    }
}

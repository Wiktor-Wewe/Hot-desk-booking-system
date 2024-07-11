using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Employees.Queries;
using Hdbs.Transfer.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<PaginatedList<EmployeeListDto>> ListAsync(ListEmployeesQuery listAsyncQuery);
        Task<EmployeeDto> GetAsync(GetEmployeeQuery query);
    }
}

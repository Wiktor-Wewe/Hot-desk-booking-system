using Hdbs.Transfer.Employees.Data;
using Hdbs.Transfer.Shared.Data;
using Hdbs.Transfer.Shared.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Queries
{
    public class ListEmployeesQuery : ListQuery, IRequest<PaginatedList<EmployeeListDto>>
    {
    }
}

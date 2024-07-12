using Hdbs.Transfer.Employees.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Queries
{
    public class GetEmployeeQuery : IRequest<EmployeeDto>
    {
        public string Id { get; set; } = null!;
    }
}

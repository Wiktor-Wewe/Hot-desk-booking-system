using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Commands
{
    public class SetStatusForEmployeeCommand : IRequest
    {
        public string? EmployeeId { get; set; }
        public bool IsDisabled { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Commands
{
    public class DeleteEmployeeCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}

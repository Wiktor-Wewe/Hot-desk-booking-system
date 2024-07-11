using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Commands
{
    public class UpdateEmployeeCommand : IRequest
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
    }
}

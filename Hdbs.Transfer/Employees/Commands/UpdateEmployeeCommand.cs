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
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
        public string? NewPassword { get; set; }
        public string? RePassword { get; set; }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Commands
{
    public class SetPermissionsForEmployeeCommand : IRequest
    {
        public string? Id { get; set; }

        public bool None { get; set; } = false;
        public bool SimpleView { get; set; } = true;
        public bool AdminView { get; set; } = false;

        public bool CreateEmployee { get; set; } = false;
        public bool UpdateEmployee { get; set; } = false;
        public bool DeleteEmployee { get; set; } = false;

        public bool CreateLocation { get; set; } = false;
        public bool UpdateLocation { get; set; } = false;
        public bool DeleteLocation { get; set; } = false;

        public bool CreateDesk { get; set; } = false;
        public bool UpdateDesk { get; set; } = false;
        public bool DeleteDesk { get; set; } = false;

        public bool SetPermissions { get; set; } = false;
        public bool SetEmployeeStatus { get; set; } = false;

        public bool CreateReservation { get; set; } = false;
        public bool UpdateReservation { get; set; } = false;
        public bool DeleteReservation { get; set; } = false;
    }
}

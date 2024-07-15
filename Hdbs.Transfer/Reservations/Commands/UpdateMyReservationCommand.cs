using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Reservations.Commands
{
    public class UpdateMyReservationCommand : IRequest
    {
        public string? EmployeeId { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid DeskId { get; set; }
    }
}

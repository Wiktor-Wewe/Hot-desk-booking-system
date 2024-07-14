using Hdbs.Transfer.Reservations.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Desks.Commands
{
    public class ReserveDeskCommand : IRequest<ReservationDto>
    {
        public Guid? DeskId { get; set; }
        public string? EmployeeId {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

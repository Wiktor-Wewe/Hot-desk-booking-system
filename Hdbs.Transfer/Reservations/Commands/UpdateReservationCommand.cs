using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Reservations.Commands
{
    public class UpdateReservationCommand : IRequest
    {
        public Guid? Id {  get; set; }
        public Guid DeskId { get; set; }
    }
}

using Hdbs.Transfer.Reservations.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Reservations.Queries
{
    public class GetReservationQuery : IRequest<ReservationDto>
    {
        public Guid Id { get; set; }
    }
}

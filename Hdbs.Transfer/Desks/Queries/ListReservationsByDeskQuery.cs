using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Shared.Data;
using Hdbs.Transfer.Shared.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Desks.Queries
{
    public class ListReservationsByDeskQuery : ListQuery, IRequest<PaginatedList<ReservationListDto>>
    {
        public Guid? DeskId { get; set; }
        public bool OnlyActive { get; set; } = false;
    }
}

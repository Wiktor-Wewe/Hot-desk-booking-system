using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Shared.Data;
using Hdbs.Transfer.Shared.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Locations.Queries
{
    public class ListReservationsByLocationQuery : ListQuery, IRequest<PaginatedList<ReservationListDto>>
    {
        public Guid? LocationId { get; set; }
    }
}

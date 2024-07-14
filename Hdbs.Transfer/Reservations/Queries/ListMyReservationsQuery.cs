using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Shared.Data;
using Hdbs.Transfer.Shared.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Reservations.Queries
{
    public class ListMyReservationsQuery : ListQuery, IRequest<PaginatedList<ReservationListDto>>
    {
        public string? EmployeeId { get; set; }
        public bool OnlyActive { get; set; } = false;
    }
}

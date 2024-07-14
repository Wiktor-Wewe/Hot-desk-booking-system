using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Desks.Queries;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Interfaces
{
    public interface IDeskRepository
    {
        Task<PaginatedList<DeskListDto>> ListAsync(ListDesksQuery listAsyncQuery);
        Task<PaginatedList<ReservationListDto>> ListReservationsAsync(ListReservationsByDeskQuery listAsyncQuery);
        Task<DeskDto> GetAsync(GetDeskQuery query);
    }
}

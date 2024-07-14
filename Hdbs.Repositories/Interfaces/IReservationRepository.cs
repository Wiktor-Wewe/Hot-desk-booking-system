using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Reservations.Queries;
using Hdbs.Transfer.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task<PaginatedList<ReservationListDto>> ListAsync(ListReservationsQuery listAsyncQuery);
        Task<ReservationDto> GetAsync(GetReservationQuery query);
    }
}

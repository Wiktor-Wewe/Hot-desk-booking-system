using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Shared.Data;

namespace Hdbs.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Task<PaginatedList<LocationListDto>> ListAsync(ListLocationsQuery listAsyncQuery);
        Task<PaginatedList<DeskListDto>> ListDesksAsync(ListDesksByLocationQuery listAsyncQuery);
        Task<PaginatedList<ReservationListDto>> ListReservationsAsync(ListReservationsByLocationQuery listAsyncQuery);
        Task<LocationDto> GetAsync(GetLocationQuery query);
    }
}

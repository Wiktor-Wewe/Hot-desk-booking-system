using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Task<PaginatedList<LocationListDto>> ListAsync(ListLocationsQuery listyAsyncQuery);
        Task<LocationDto> GetAsync(GetLocationQuery query);
    }
}

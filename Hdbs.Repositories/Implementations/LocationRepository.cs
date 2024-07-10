using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Repositories.Implementations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly HdbsContext _dbContext;
        
        public LocationRepository(HdbsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<LocationListDto>> ListAsync(ListLocationsQuery listyAsyncQuery)
        {
            var query = _dbContext.Locations
                .AsNoTracking()
                .OrderBy(p => p.Id);

            return await PaginatedList<LocationListDto>.CreateAsync(query.Select(p => new LocationListDto
            {
                Id = p.Id,
                Name = p.Name,
                Desks = p.Desks
            }).AsQueryable()
                .AsNoTracking(),
                listyAsyncQuery.PageIndex,
                listyAsyncQuery.PageSize
            );
        }

        public async Task<LocationDto> GetAsync(GetLocationQuery query)
        {
            var location = await _dbContext.Locations
                .FirstOrDefaultAsync(p => p.Id == query.Id);

            if (location == null)
            {
                throw new CustomException(CustomErrorCode.LocationNotFound, $"Unable to find location with id: {query.Id}");
            }

            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Desks = location.Desks
            };
        } 
    }
}

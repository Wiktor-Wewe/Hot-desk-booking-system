using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Core.Utils;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hdbs.Repositories.Implementations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly HdbsContext _dbContext;
        
        public LocationRepository(HdbsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<LocationListDto>> ListAsync(ListLocationsQuery listAsyncQuery)
        {
            var query = _dbContext.Locations
                .Include(l => l.Desks)
                .AsNoTracking()
                .OrderBy(l => l.Id);

            if (!string.IsNullOrEmpty(listAsyncQuery.SearchFor) && !string.IsNullOrEmpty(listAsyncQuery.SearchBy))
            {
                if (Utils.IsValidProperty<Location>(listAsyncQuery.SearchBy) == false)
                {
                    throw new CustomException(CustomErrorCode.InvalidSearchBy, $"Unable to search by: {listAsyncQuery.SearchBy}");
                }

                listAsyncQuery.SearchFor = listAsyncQuery.SearchFor.Replace("'", "''");
                query = (IOrderedQueryable<Location>)query.Where($"{listAsyncQuery.SearchBy}.Contains(@0)", listAsyncQuery.SearchFor);
            }

            if (!string.IsNullOrEmpty(listAsyncQuery.OrderBy))
            {
                if (Utils.IsValidProperty<Location>(listAsyncQuery.OrderBy) == false)
                {
                    throw new CustomException(CustomErrorCode.InvalidOrderBy, $"Unable to order by: {listAsyncQuery.OrderBy}");
                }

                query = listAsyncQuery.Ascending
                    ? query.OrderBy(listAsyncQuery.OrderBy)
                    : query.OrderBy($"{listAsyncQuery.OrderBy} descending");
            }

            return await PaginatedList<LocationListDto>.CreateAsync(query.Select(l => new LocationListDto
            {
                Id = l.Id,
                Name = l.Name,
                Description = l.Description,
                Address = l.Address,
                City = l.City,
                Country = l.City,
                Desks = l.Desks
            }).AsQueryable()
                .AsNoTracking(),
                listAsyncQuery.PageIndex,
                listAsyncQuery.PageSize
            );
        }

        public async Task<PaginatedList<DeskListDto>> ListDesksAsync(ListDesksByLocationQuery listAsyncQuery)
        {
            var query = _dbContext.Desks
                .Include(d => d.Location)
                .Include(d => d.Reservations)
                .Where(d => d.LocationId == listAsyncQuery.LocationId)
                .AsNoTracking()
                .OrderBy(d => d.Id);

            if (!string.IsNullOrEmpty(listAsyncQuery.SearchFor) && !string.IsNullOrEmpty(listAsyncQuery.SearchBy))
            {
                if (Utils.IsValidProperty<Desk>(listAsyncQuery.SearchBy) == false)
                {
                    throw new CustomException(CustomErrorCode.InvalidSearchBy, $"Unable to search by: {listAsyncQuery.SearchBy}");
                }

                listAsyncQuery.SearchFor = listAsyncQuery.SearchFor.Replace("'", "''");
                query = (IOrderedQueryable<Desk>)query.Where($"{listAsyncQuery.SearchBy}.Contains(@0)", listAsyncQuery.SearchFor);
            }

            if (!string.IsNullOrEmpty(listAsyncQuery.OrderBy))
            {
                if (Utils.IsValidProperty<Desk>(listAsyncQuery.OrderBy) == false)
                {
                    throw new CustomException(CustomErrorCode.InvalidOrderBy, $"Unable to order by: {listAsyncQuery.OrderBy}");
                }

                query = listAsyncQuery.Ascending
                    ? query.OrderBy(listAsyncQuery.OrderBy)
                    : query.OrderBy($"{listAsyncQuery.OrderBy} descending");
            }

            var desks = await PaginatedList<DeskListDto>.CreateAsync(query.Select(d => new DeskListDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                LocationId = d.LocationId,
                Location = d.Location,
                IsAvailable = false,
                Reservations = d.Reservations

            }).AsQueryable()
                .AsNoTracking(),
                listAsyncQuery.PageIndex,
                listAsyncQuery.PageSize
            );

            foreach (var desk in desks)
            {
                desk.IsAvailable = desk.Reservations?.LastOrDefault(r => r.IsFreeRightNow() == false) == null ? true : false;
            }

            return desks;
        }

        public async Task<LocationDto> GetAsync(GetLocationQuery query)
        {
            var location = await _dbContext.Locations
                .Include(l => l.Desks)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == query.Id);

            if (location == null)
            {
                throw new CustomException(CustomErrorCode.LocationNotFound, $"Unable to find location with id: {query.Id}");
            }

            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Description = location.Description,
                Address = location.Address,
                City = location.City,
                Country = location.Country,
                Desks = location.Desks
            };
        }
    }
}

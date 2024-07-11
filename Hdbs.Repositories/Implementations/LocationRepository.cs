﻿using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Core.Utils;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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

        public async Task<PaginatedList<LocationListDto>> ListAsync(ListLocationsQuery listAsyncQuery)
        {
            var query = _dbContext.Locations
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

        public async Task<LocationDto> GetAsync(GetLocationQuery query)
        {
            var location = await _dbContext.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == query.Id);

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

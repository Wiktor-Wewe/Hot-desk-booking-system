﻿using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Core.Utils;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Desks.Queries;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Hdbs.Repositories.Implementations
{
    public class DeskRepository : IDeskRepository
    {
        private readonly HdbsContext _dbContext;

        public DeskRepository(HdbsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeskDto> GetAsync(GetDeskQuery query)
        {
            var desk = await _dbContext.Desks
                .Include(d => d.Location)
                .Include(d => d.Reservations)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == query.Id);

            if (desk == null)
            {
                throw new CustomException(CustomErrorCode.DeskNotFound, $"Unable to find desk with id: {query.Id}");
            }

            if (query.StartDate == null) query.StartDate = DateTime.Now;
            if (query.EndDate == null) query.EndDate = DateTime.Now;
            if (query.StartDate > query.EndDate)
            {
                var temp = query.StartDate;
                query.StartDate = query.EndDate;
                query.EndDate = temp;
            }

            return new DeskDto
            {
                Id = desk.Id,
                Name = desk.Name,
                Description = desk.Description,
                LocationId = desk.LocationId,
                LocationName = desk.Location.Name,
                LocationCity = desk.Location.City,
                LocationCountry = desk.Location.Country,
                IsAvailable = desk.Reservations.LastOrDefault(r => (query.EndDate.Value.Date < r.StartDate.Date || query.StartDate.Value.Date > r.EndDate.Date) == false) == null
            };
        }

        public async Task<PaginatedList<DeskListDto>> ListAsync(ListDesksQuery listAsyncQuery)
        {
            var query = _dbContext.Desks
                .Include(d => d.Location)
                .Include(d => d.Reservations)
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
                if(Utils.IsValidProperty<Desk>(listAsyncQuery.OrderBy) == false)
                {
                    throw new CustomException(CustomErrorCode.InvalidOrderBy, $"Unable to order by: {listAsyncQuery.OrderBy}");
                }

                query = listAsyncQuery.Ascending
                    ? query.OrderBy(listAsyncQuery.OrderBy)
                    : query.OrderBy($"{listAsyncQuery.OrderBy} descending");
            }

            if (listAsyncQuery.StartDate == null) listAsyncQuery.StartDate = DateTime.Now;
            if (listAsyncQuery.EndDate == null) listAsyncQuery.EndDate = DateTime.Now;
            if (listAsyncQuery.StartDate > listAsyncQuery.EndDate)
            {
                var temp = listAsyncQuery.StartDate;
                listAsyncQuery.StartDate = listAsyncQuery.EndDate;
                listAsyncQuery.EndDate = temp;
            }

            return await PaginatedList<DeskListDto>.CreateAsync(query.Select(d => new DeskListDto
            {
                Id = d.Id,
                Name = d.Name,
                LocationId = d.LocationId,
                LocationName = d.Location.Name,
                IsAvailable = d.Reservations.LastOrDefault(r => (listAsyncQuery.EndDate.Value.Date < r.StartDate.Date || listAsyncQuery.StartDate.Value.Date > r.EndDate.Date) == false) == null
            }).AsQueryable()
                .AsNoTracking(),
                listAsyncQuery.PageIndex,
                listAsyncQuery.PageSize
            );
        }
    }
}

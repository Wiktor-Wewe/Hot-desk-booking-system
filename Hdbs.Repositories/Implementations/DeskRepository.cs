using Hdbs.Core.CustomExceptions;
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

            return new DeskDto
            {
                Id = desk.Id,
                Name = desk.Name,
                Description = desk.Description,
                LocationId = desk.LocationId,
                Location = desk.Location,
                IsAvailable = desk.Reservations?.LastOrDefault(r => r.IsFreeRightNow() == false) == null ? true : false,
                Reservations = desk.Reservations
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

            foreach(var desk in desks)
            {
                desk.IsAvailable = desk.Reservations?.LastOrDefault(r => r.IsFreeRightNow() == false) == null ? true : false;
            }

            return desks;
        }
    }
}

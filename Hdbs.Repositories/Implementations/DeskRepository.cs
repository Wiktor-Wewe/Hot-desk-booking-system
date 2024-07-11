using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Desks.Queries;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .FirstOrDefaultAsync(p => p.Id == query.Id);

            if (desk == null)
            {
                throw new CustomException(CustomErrorCode.DeskNotFound, $"Unable to find desk with id: {query.Id}");
            }

            return new DeskDto
            {
                Id = desk.Id,
                Name = desk.Name,
                LocationId = desk.LocationId,
                Location = desk.Location,
                IsAvailable = desk.Reservations?.FirstOrDefault(r => r.IsValid()) == null ? true : false,
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

            var desks = await PaginatedList<DeskListDto>.CreateAsync(query.Select(d => new DeskListDto
            {
                Id = d.Id,
                Name = d.Name,
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
                desk.IsAvailable = desk.Reservations?.FirstOrDefault(r => r.IsValid()) == null ? true : false;
            }

            return desks;
        }
    }
}

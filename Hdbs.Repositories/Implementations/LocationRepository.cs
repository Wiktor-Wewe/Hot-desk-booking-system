using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Core.Utils;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Reservations.Data;
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

            query = (IOrderedQueryable<Location>)PaginatedList<Location>.ApplySearchAndSorting(query, listAsyncQuery.SearchBy, listAsyncQuery.SearchFor, listAsyncQuery.OrderBy, listAsyncQuery.Ascending);

            return await PaginatedList<LocationListDto>.CreateAsync(query.Select(l => new LocationListDto
            {
                Id = l.Id,
                Name = l.Name,
                City = l.City,
                Country = l.Country
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

            query = (IOrderedQueryable<Desk>)PaginatedList<Desk>.ApplySearchAndSorting(query, listAsyncQuery.SearchBy, listAsyncQuery.SearchFor, listAsyncQuery.OrderBy, listAsyncQuery.Ascending);

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

        public async Task<PaginatedList<ReservationListDto>> ListReservationsAsync(ListReservationsByLocationQuery listAsyncQuery)
        {
            if(listAsyncQuery.LocationId == null)
            {
                throw new CustomException(CustomErrorCode.LocationNotFound, "Unable to find location with id: null");
            }

            var query = _dbContext.Reservations
                .Include(r => r.Desk)
                .Include(r => r.Employee)
                .Where(r => r.Desk.LocationId == listAsyncQuery.LocationId.Value)
                .AsNoTracking()
                .OrderBy(d => d.Id);

            query = (IOrderedQueryable<Reservation>)PaginatedList<Reservation>.ApplySearchAndSorting(query, listAsyncQuery.SearchBy, listAsyncQuery.SearchFor, listAsyncQuery.OrderBy, listAsyncQuery.Ascending);

            return await PaginatedList<ReservationListDto>.CreateAsync(query.Select(d => new ReservationListDto
            {
                Id = d.Id,
                DeskId = d.DeskId,
                LocationName = d.Desk.Location.Name,
                LocationCity = d.Desk.Location.City,
                LocationCountry = d.Desk.Location.Country,
                EmployeeId = d.EmployeeId,
                EmployeeName = d.Employee.UserName == null ? "" : d.Employee.UserName,
                EmployeeSurname = d.Employee.Surname,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                IsExpired = d.IsExpiredRightNow(),
                IsActive = !d.IsFreeRightNow()

            }).AsQueryable()
                .AsNoTracking(),
                listAsyncQuery.PageIndex,
                listAsyncQuery.PageSize
            );
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
                Country = location.Country
            };
        }
    }
}

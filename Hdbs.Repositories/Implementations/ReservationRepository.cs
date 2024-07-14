using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Core.Utils;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Reservations.Queries;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Dynamic.Core;

namespace Hdbs.Repositories.Implementations
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HdbsContext _dbContext;

        public ReservationRepository(HdbsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReservationDto> GetAsync(GetReservationQuery query)
        {
            var reservation = await _dbContext.Reservations
                .Include(r => r.Desk)
                .ThenInclude(d => d.Location)
                .Include(r => r.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == query.Id);

            if(reservation == null)
            {
                throw new CustomException(CustomErrorCode.ReservationNotFound, $"Unable to find reservation with id: {query.Id}");
            }

            return new ReservationDto
            {
                Id = reservation.Id,
                DeskId = reservation.DeskId,
                DeskName = reservation.Desk.Name,
                LocationName = reservation.Desk.Location.Name,
                LocationCity = reservation.Desk.Location.City,
                LocationCountry = reservation.Desk.Location.Country,
                EmployeeId = reservation.EmployeeId,
                EmployeeName = reservation.Employee.UserName == null ? "" : reservation.Employee.UserName,
                EmployeeSurname = reservation.Employee.Surname,
                EmployeeEmail = reservation.Employee.Email == null ? "" : reservation.Employee.Email,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                IsExpired = reservation.IsExpiredRightNow(),
                IsActive = !reservation.IsFreeRightNow()
            };
        }

        public async Task<PaginatedList<ReservationListDto>> ListAsync(ListReservationsQuery listAsyncQuery)
        {
            var query = _dbContext.Reservations
                .Include(d => d.Desk)
                .Include(d => d.Employee)
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

        public async Task<PaginatedList<ReservationListDto>> ListMyReservationsAsync(ListMyReservationsQuery listAsyncQuery)
        {
            if(listAsyncQuery.EmployeeId == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeIdIsNull, "Unable to find employee with id: null");
            }

            var query = _dbContext.Reservations
                .Include(r => r.Desk)
                .Include(r => r.Employee)
                .Where(r => r.EmployeeId == listAsyncQuery.EmployeeId)
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
    }
}

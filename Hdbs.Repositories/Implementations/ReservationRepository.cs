using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Core.Utils;
using Hdbs.Data.Models;
using Hdbs.Repositories.Interfaces;
using Hdbs.Transfer.Reservations.Data;
using Hdbs.Transfer.Reservations.Queries;
using Hdbs.Transfer.Shared.Data;
using Microsoft.EntityFrameworkCore;
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
                Desk = reservation.Desk,
                EmployeeId = reservation.EmployeeId,
                Employee = reservation.Employee,
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

            if (!string.IsNullOrEmpty(listAsyncQuery.SearchFor) && !string.IsNullOrEmpty(listAsyncQuery.SearchBy))
            {
                if (Utils.IsValidProperty<Reservation>(listAsyncQuery.SearchBy) == false)
                {
                    throw new CustomException(CustomErrorCode.InvalidSearchBy, $"Unable to search by: {listAsyncQuery.SearchBy}");
                }

                listAsyncQuery.SearchFor = listAsyncQuery.SearchFor.Replace("'", "''");
                query = (IOrderedQueryable<Reservation>)query.Where($"{listAsyncQuery.SearchBy}.Contains(@0)", listAsyncQuery.SearchFor);
            }

            if (!string.IsNullOrEmpty(listAsyncQuery.OrderBy))
            {
                if (Utils.IsValidProperty<Reservation>(listAsyncQuery.OrderBy) == false)
                {
                    throw new CustomException(CustomErrorCode.InvalidOrderBy, $"Unable to order by: {listAsyncQuery.OrderBy}");
                }

                query = listAsyncQuery.Ascending
                    ? query.OrderBy(listAsyncQuery.OrderBy)
                    : query.OrderBy($"{listAsyncQuery.OrderBy} descending");
            }

            return await PaginatedList<ReservationListDto>.CreateAsync(query.Select(d => new ReservationListDto
            {
                Id = d.Id,
                DeskId = d.DeskId,
                Desk = d.Desk,
                EmployeeId = d.EmployeeId,
                Employee = d.Employee,
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

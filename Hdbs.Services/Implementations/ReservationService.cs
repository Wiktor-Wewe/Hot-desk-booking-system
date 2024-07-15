using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Reservations.Commands;
using Hdbs.Transfer.Reservations.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hdbs.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        public readonly HdbsContext _dbContext;
        private readonly UserManager<Employee> _userManager;

        public ReservationService(HdbsContext dbContext, UserManager<Employee> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<ReservationDto> CreateAsync(CreateReservationCommand command)
        {
            var employee = await _userManager.FindByIdAsync(command.EmployeeId);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, $"Unable to find employee with id: {command.EmployeeId}");
            }

            var desk = await _dbContext.Desks
                .Include(d => d.Reservations)
                .FirstOrDefaultAsync(l => l.Id == command.DeskId);

            if(desk == null)
            {
                throw new CustomException(CustomErrorCode.DeskNotFound, $"Unable to find desk with id: {command.DeskId}");
            }

            if (desk.ForcedUnavailable)
            {
                throw new CustomException(CustomErrorCode.DeskIsUnavailable, $"Unable to make reservation for desk with id: {command.DeskId} - desk is unavaible for this moment");
            }

            if (desk.Reservations.LastOrDefault(r => r.IsFree(command.StartDate, command.EndDate) == false) != null)
            {
                throw new CustomException(CustomErrorCode.DeskIsUnavailable, $"Unable to make reservation for desk with id: {command.DeskId} - desk is unavaible for this moment");
            }

            var reservation = new Reservation
            {
                DeskId = command.DeskId,
                Desk = desk,
                EmployeeId = command.EmployeeId,
                Employee = employee,
                StartDate = command.StartDate,
                EndDate = command.EndDate,
            };

            if(reservation.IsValid() == false)
            {
                throw new CustomException(CustomErrorCode.ReservationIsImpossible, $"Unable to make reservation for desk with id: {command.DeskId} - reservation is impossible");
            }

            await _dbContext.Reservations.AddAsync(reservation);
            await _dbContext.SaveOrHandleExceptionAsync();

            var reservationFromDb = await _dbContext.Reservations
                .Include(r => r.Desk)
                .Include(r => r.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == reservation.Id);

            if (reservationFromDb == null) 
            {
                throw new CustomException(CustomErrorCode.ReservationNotFound, $"Unable to find reservation with id: {command.DeskId}");
            }

            return new ReservationDto
            {
                Id = reservationFromDb.Id,
                DeskId = reservationFromDb.DeskId,
                DeskName = reservationFromDb.Desk.Name,
                LocationName = reservationFromDb.Desk.Location.Name,
                LocationCity = reservationFromDb.Desk.Location.City,
                LocationCountry = reservationFromDb.Desk.Location.Country,
                EmployeeId = reservationFromDb.EmployeeId,
                EmployeeName = reservationFromDb.Employee.UserName == null ? "" : reservationFromDb.Employee.UserName,
                EmployeeSurname = reservationFromDb.Employee.Surname,
                EmployeeEmail = reservationFromDb.Employee.Email == null ? "" : reservationFromDb.Employee.Email,
                StartDate = reservationFromDb.StartDate,
                EndDate = reservationFromDb.EndDate,
                IsExpired = reservationFromDb.IsExpiredRightNow(),
                IsActive = !reservationFromDb.IsFreeRightNow()
            };
        }

        public async Task DeleteAsync(DeleteReservationCommand command)
        {
            var reservation = await _dbContext.Reservations
                .FirstOrDefaultAsync(r => r.Id == command.Id);

            if(reservation == null)
            {
                throw new CustomException(CustomErrorCode.ReservationNotFound, $"Unable to find reservation with id: {command.Id}");
            }

            _dbContext.Reservations.Remove(reservation);
            await _dbContext.SaveOrHandleExceptionAsync();
        }

        public async Task UpdateAsync(UpdateReservationCommand command)
        {
            var reservation = await _dbContext.Reservations
                .Include(r => r.Employee)
                .Include(r => r.Desk)
                .FirstOrDefaultAsync(r => r.Id == command.Id);

            if (reservation == null)
            {
                throw new CustomException(CustomErrorCode.ReservationNotFound, $"Unable to find reservation with id: {command.Id}");
            }

            if (DateTime.Now > reservation.StartDate || reservation.StartDate - DateTime.Now > new TimeSpan(24, 0, 0))
            {
                throw new CustomException(CustomErrorCode.TooLateToUpdateReservation, $"Unable to update reservation with id: {command.Id} - it's too late");
            }

            var desk = await _dbContext.Desks
                .FirstOrDefaultAsync(d => d.Id == command.DeskId);

            if (desk == null)
            {
                throw new CustomException(CustomErrorCode.DeskNotFound, $"Unable to find desk with id: {command.Id}");
            }

            if (desk.ForcedUnavailable)
            {
                throw new CustomException(CustomErrorCode.DeskIsUnavailable, $"Unable to update reservation with id: {command.Id} - desk is unavaible for this moment");
            }

            if (desk.Reservations.LastOrDefault(r => r.IsFree(reservation.StartDate, reservation.EndDate) == false) != null)
            {
                throw new CustomException(CustomErrorCode.DeskIsUnavailable, $"Unable to update reservation with id: {command.Id} - desk is unavaible for this moment");
            }

            reservation.DeskId = desk.Id;
            reservation.Desk = desk;
            await _dbContext.SaveOrHandleExceptionAsync();
        }
    }
}

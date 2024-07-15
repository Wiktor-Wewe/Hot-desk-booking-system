using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Desks.Commands;
using Hdbs.Transfer.Desks.Data;
using Hdbs.Transfer.Reservations.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hdbs.Services.Implementations
{
    public class DeskService : IDeskService
    {
        private readonly HdbsContext _dbContext;
        private readonly UserManager<Employee> _userManager;

        public DeskService(HdbsContext dbContext, UserManager<Employee> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<DeskDto> CreateAsync(CreateDeskCommand command)
        {
            var location = await _dbContext.Locations
                .FirstOrDefaultAsync(l => l.Id == command.LocationId);

            if(location == null)
            {
                throw new CustomException(CustomErrorCode.LocationNotFound, $"Unable to find location with id: {command.LocationId}");
            }

            var desk = new Desk
            {
                ForcedUnavailable = false,
                Name = command.Name,
                Description = command.Description,
                LocationId = command.LocationId,
                Location = location
            };

            await _dbContext.Desks.AddAsync(desk);
            await _dbContext.SaveOrHandleExceptionAsync();

            var deskFromDb = await _dbContext.Desks
                .Include(d => d.Location)
                .Include(d => d.Reservations)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == desk.Id);

            if(deskFromDb == null)
            {
                throw new CustomException(CustomErrorCode.DeskNotFound, $"Unable to find desk with id: {desk.Id}");
            }

            return new DeskDto
            {
                Id = deskFromDb.Id,
                Name = deskFromDb.Name,
                Description = deskFromDb.Description,
                LocationId = deskFromDb.LocationId,
                LocationName = deskFromDb.Location.Name,
                LocationCity = deskFromDb.Location.City,
                LocationCountry = deskFromDb.Location.Country,
                IsAvailable = deskFromDb.Reservations?.LastOrDefault(r => r.IsFreeRightNow() == false) == null ? true : false
            };
        }

        public async Task<ReservationDto> ReserveDeskAsync(ReserveDeskCommand command)
        {
            if(command.EmployeeId == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeIdIsNull, "Unable to make reservation - Employee Id is null");
            }

            if(command.DeskId == null)
            {
                throw new CustomException(CustomErrorCode.DeskIdIsNull, "Unable to make reservation - Employee Id is null");
            }

            var employee = await _userManager.FindByIdAsync(command.EmployeeId);

            if (employee == null)
            {
                throw new CustomException(CustomErrorCode.EmployeeNotFound, "Unable to make reservation - Desk Id is null");
            }

            var desk = await _dbContext.Desks
                .Include(d => d.Reservations)
                .FirstOrDefaultAsync(l => l.Id == command.DeskId);

            if (desk == null)
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
                DeskId = command.DeskId.Value,
                Desk = desk,
                EmployeeId = command.EmployeeId,
                Employee = employee,
                StartDate = command.StartDate,
                EndDate = command.EndDate,
            };

            if (reservation.IsValid() == false)
            {
                throw new CustomException(CustomErrorCode.ReservationIsImpossible, $"Unable to make reservation for desk with id: {command.DeskId} - reservation is impossible");
            }

            await _dbContext.Reservations.AddAsync(reservation);
            await _dbContext.SaveOrHandleExceptionAsync();

            var reservationFromDb = await _dbContext.Reservations
                .Include(r => r.Desk)
                .ThenInclude(d => d.Location)
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

        public async Task DeleteAsync(DeleteDeskCommand command)
        {
            var desk = await _dbContext.Desks
                .FirstOrDefaultAsync(d => d.Id == command.Id);

            if (desk == null)
            {
                throw new CustomException(CustomErrorCode.DeskNotFound, $"Unable to find desk with id: {command.Id}");
            }

            if (desk.Reservations?.LastOrDefault(r => r.IsFreeRightNow() == false) != null)
            {
                throw new CustomException(CustomErrorCode.DeskIsUnavailable, $"Unable to delete desk with id: {command.Id} - Desk is unavailable right now");
            }

            _dbContext.Desks.Remove(desk);
            await _dbContext.SaveOrHandleExceptionAsync();
        }

        public async Task UpdateAsync(UpdateDeskCommand command)
        {
            var desk = await _dbContext.Desks
                .FirstOrDefaultAsync(d => d.Id == command.Id);

            if (desk == null)
            {
                throw new CustomException(CustomErrorCode.DeskNotFound, $"Unable to find desk with id: {command.Id}");
            }

            if (desk.Reservations?.LastOrDefault(r => r.IsFreeRightNow() == false) != null)
            {
                throw new CustomException(CustomErrorCode.DeskIsUnavailable, $"Unable to edit desk with id: {command.Id} - Desk is unavailable right now");
            }

            desk.ForcedUnavailable = command.ForcedUnavailable == null ? desk.ForcedUnavailable : command.ForcedUnavailable.Value;
            desk.Name = command.Name == null ? desk.Name : command.Name;
            desk.Description = command.Description == null ? desk.Description : command.Description;
            desk.LocationId = command.LocationId == null ? desk.LocationId : command.LocationId.Value;

            await _dbContext.SaveOrHandleExceptionAsync();
        }
    }
}

using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Desks.Commands;
using Hdbs.Transfer.Desks.Data;
using Microsoft.EntityFrameworkCore;

namespace Hdbs.Services.Implementations
{
    public class DeskService : IDeskService
    {
        private readonly HdbsContext _dbContext;

        public DeskService(HdbsContext dbContext)
        {
            _dbContext = dbContext;
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
                Location = deskFromDb.Location,
                IsAvailable = deskFromDb.Reservations?.FirstOrDefault(r => r.IsValid()) == null ? true : false,
                Reservations = deskFromDb.Reservations
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

            if(desk.Reservations?.FirstOrDefault(r => r.IsValid()) != null)
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

            desk.Name = command.Name == null ? desk.Name : command.Name;
            desk.Description = command.Description == null ? desk.Description : command.Description;
            desk.LocationId = command.LocationId == null ? desk.LocationId : command.LocationId.Value;

            await _dbContext.SaveOrHandleExceptionAsync();
        }
    }
}

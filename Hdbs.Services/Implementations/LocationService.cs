using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Hdbs.Services.Interfaces;
using Hdbs.Transfer.Locations.Commands;
using Hdbs.Transfer.Locations.Data;
using Microsoft.EntityFrameworkCore;

namespace Hdbs.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly HdbsContext _dbContext;

        public LocationService(HdbsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LocationDto> CreateAsync(CreateLocationCommand command)
        {
            var location = new Location
            {
                Name = command.Name
            };

            await _dbContext.Locations.AddAsync(location);
            await _dbContext.SaveOrHandleExceptionAsync();

            var locationFromDb = await _dbContext.Locations
                .FirstOrDefaultAsync(p => p.Id == location.Id);

            if (locationFromDb == null)
            {
                throw new CustomException(CustomErrorCode.LocationNotFound, $"Unable to find location with id: {location.Id}");
            }

            return new LocationDto
            {
                Id = locationFromDb.Id,
                Name = locationFromDb.Name,
                Desks = locationFromDb.Desks,
            };
        }

        public async Task UpdateAsync(UpdateLocationCommand command)
        {
            var location = await _dbContext.Locations
                .FirstOrDefaultAsync(p => p.Id == command.Id);

            if (location == null)
            {
                throw new CustomException(CustomErrorCode.LocationNotFound, $"Unable to find location with id: {command.Id}");
            }

            location.Name = command.Name;

            await _dbContext.SaveOrHandleExceptionAsync();
        }

        public async Task DeleteAsync(DeleteLocationCommand command)
        {
            var location = await _dbContext.Locations
                .Include(l => l.Desks)
                .FirstOrDefaultAsync(p => p.Id == command.Id);

            if (location == null)
            {
                throw new CustomException(CustomErrorCode.LocationNotFound, $"Unable to find location with id: {command.Id}");
            }

            if(location.Desks.Count > 0)
            {
                throw new CustomException(CustomErrorCode.LocationContainsDesks, $"Unable to delete location with id: {command.Id} - Location contains desks");
            }

            _dbContext.Locations.Remove(location);
            await _dbContext.SaveOrHandleExceptionAsync();
        }
    }
}

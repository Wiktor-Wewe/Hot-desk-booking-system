using Hdbs.Data.Models;
using Hdbs.Services.Implementations;
using Hdbs.Transfer.Locations.Commands;
using Microsoft.EntityFrameworkCore;

namespace Hdbs.UnitTests.Services
{
    public class LocationServiceTests
    {
        [Fact]
        public async Task CreateAsync_ValidCommand_ReturnsLocationDto()
        {
            // Arrange
            var command = new CreateLocationCommand
            {
                Name = "Test Location",
                Description = "Test Description",
                Address = "Test Address",
                City = "Test City",
                Country = "Test Country"
            };

            var dbContextOptions = new DbContextOptionsBuilder<HdbsContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            using (var dbContext = new HdbsContext(dbContextOptions))
            {
                var service = new LocationService(dbContext);

                // Act
                var result = await service.CreateAsync(command);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(command.Name, result.Name);
                Assert.Equal(command.Description, result.Description);
                Assert.Equal(command.Address, result.Address);
                Assert.Equal(command.City, result.City);
                Assert.Equal(command.Country, result.Country);
            }
        }

        [Fact]
        public async Task UpdateAsync_LocationExists_UpdatesLocation()
        {
            // Arrange
            var locationId = new Guid("6a361565-d6d8-4272-8fe4-2b8bd1d37fd2");
            var command = new UpdateLocationCommand
            {
                Id = locationId,
                Name = "Updated Location Name"
            };

            var dbContextOptions = new DbContextOptionsBuilder<HdbsContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            using (var dbContext = new HdbsContext(dbContextOptions))
            {
                var initialLocation = new Location
                {
                    Id = locationId,
                    Name = "Initial Location Name",
                    Address = "Narrow Street 12/a2",
                    City = "New York",
                    Country = "USA"
                };

                dbContext.Locations.Add(initialLocation);
                await dbContext.SaveChangesAsync();

                var service = new LocationService(dbContext);

                // Act
                await service.UpdateAsync(command);

                // Assert
                var updatedLocation = await dbContext.Locations.FindAsync(locationId);
                Assert.NotNull(updatedLocation);
                Assert.Equal(command.Name, updatedLocation.Name);
            }
        }

        [Fact]
        public async Task DeleteAsync_LocationWithNoDesks_DeletesLocation()
        {
            // Arrange
            var locationId = new Guid("6a361565-d6d8-4272-8fe4-2b8bd1d37fd2");
            var command = new DeleteLocationCommand
            {
                Id = locationId
            };

            var dbContextOptions = new DbContextOptionsBuilder<HdbsContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            using (var dbContext = new HdbsContext(dbContextOptions))
            {
                var locationToDelete = new Location
                {
                    Id = locationId,
                    Name = "Location to Delete",
                    Address = "Narrow Street 12/a2",
                    City = "New York",
                    Country = "USA"
                };

                dbContext.Locations.Add(locationToDelete);
                await dbContext.SaveChangesAsync();

                var service = new LocationService(dbContext);

                // Act
                await service.DeleteAsync(command);

                // Assert
                var deletedLocation = await dbContext.Locations.FindAsync(locationId);
                Assert.Null(deletedLocation);
            }
        }

    }
}
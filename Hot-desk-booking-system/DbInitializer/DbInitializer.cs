using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hdbs.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Hot_desk_booking_system.DbInitializer
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HdbsContext>();

                await context.Database.EnsureCreatedAsync();

                await EnsureAdminUser(serviceProvider);
            }
        }

        private static async Task EnsureAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();
            var dbContext = serviceProvider.GetRequiredService<HdbsContext>();

            if (!dbContext.Users.Any())
            {
                var admin = new Employee
                {
                    UserName = "admin",
                    Surname = "admin",
                    Email = "admin@admin.com",
                    Permissions = UserPermissions.AdminView | UserPermissions.SetPermissions | UserPermissions.CreateEmployee
                };

                var result = await userManager.CreateAsync(admin, "Admin123");

                if (!result.Succeeded)
                {
                    throw new CustomException(CustomErrorCode.UnableToCreateFirstAdmin, "Unable to create first admin");
                }
            }
        }
    }
}

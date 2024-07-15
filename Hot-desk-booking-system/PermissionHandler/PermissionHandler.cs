using Hdbs.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hot_desk_booking_system.PermissionHandler
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public UserPermissions RequiredPermission { get; }

        public PermissionRequirement(UserPermissions requiredPermission)
        {
            RequiredPermission = requiredPermission;
        }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permissionsClaim = context.User.Claims.FirstOrDefault(c => c.Type == "permissions");
            if (permissionsClaim != null && int.TryParse(permissionsClaim.Value, out int permissions))
            {
                if ((permissions & (int)requirement.RequiredPermission) == (int)requirement.RequiredPermission)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}

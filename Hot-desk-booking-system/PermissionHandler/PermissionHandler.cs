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
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            var permissionsClaim = context.User.FindFirst(c => c.Type == "permissions")?.Value;
            if (permissionsClaim == null)
            {
                return Task.CompletedTask;
            }

            if (Enum.TryParse(permissionsClaim, out UserPermissions userPermissions))
            {
                if ((userPermissions & requirement.RequiredPermission) != 0)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}

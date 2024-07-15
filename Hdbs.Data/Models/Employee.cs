using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hdbs.Data.Models
{
    [Flags]
    public enum UserPermissions
    {
        None = 0,
        SimpleView = 1 << 0,
        AdminView = 1 << 1,

        CreateEmployee = 1 << 2,
        UpdateEmployee = 1 << 3,
        DeleteEmployee = 1 << 4,

        CreateLocation = 1 << 5,
        UpdateLocation = 1 << 6,
        DeleteLocation = 1 << 7,

        CreateDesk = 1 << 8,
        UpdateDesk = 1 << 9,
        DeleteDesk = 1 << 10,

        SetPermissions = 1 << 11,
        SetEmployeeStatus = 1 << 12,

        CreateReservation = 1 << 13,
        UpdateReservation = 1 << 14,
        DeleteReservation = 1 << 15,
    }

    public class Employee : IdentityUser
    {
        [Required]
        public bool IsDisabled { get; set; } = false;
        [Required]
        public string Surname { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; } = [];
        public UserPermissions Permissions { get; set; }
    }

    public class PermissionUtils
    {
        public static bool HasPermission(string permissionsString, UserPermissions permissionToCheck)
        {
            var permissions = (UserPermissions)Enum.Parse(typeof(UserPermissions), permissionsString);
            return (permissions & permissionToCheck) == permissionToCheck;
        }
    }
}

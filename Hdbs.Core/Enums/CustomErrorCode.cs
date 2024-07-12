using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Core.Enums
{
    public enum CustomErrorCode
    {
        UnexpectedError = 400,
        LocationNotFound = 401,
        LocationContainsDesks = 402,
        DeskNotFound = 403,
        DeskIsUnavailable = 404,
        EmployeeNotFound = 405,
        InvalidSearchBy = 406,
        InvalidOrderBy = 407,
        PasswordsNotMatch = 408,
        UnableToCreateEmployee = 409,
        UnableToDeleteEmployee = 410,
        UnableToUpdatePassword = 411,
        UnableToUpdateEmployee = 412,
        WrongPassword = 413,
        NoJwtSecretKey = 414,
        PermissionError = 415,
    }
}

﻿using System;
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
    }
}

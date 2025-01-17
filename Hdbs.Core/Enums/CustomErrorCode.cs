﻿namespace Hdbs.Core.Enums
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
        ReservationNotFound = 416,
        ReservationIsImpossible = 417,
        TooLateToUpdateReservation = 418,
        EmployeeIdIsNull = 419,
        DeskIdIsNull = 420,
        WrongDateTimeFormat = 421,
        WrongGuidFormat = 422,
        UnableToCreateFirstAdmin = 423,
        WrongBoolFormat = 424,
        NotMatchingIds = 425,
        NoFileUploaded = 426,
        InvalidFileFormat = 427,
        InvalidTemplateFormat = 428
    }
}

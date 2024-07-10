using Hdbs.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Core.CustomExceptions
{
    public class CustomException : Exception
    {
        public CustomErrorCode ErrorCode { get; private set; }
        public string? Message { get; private set; }

        public CustomException(CustomErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public CustomException(CustomErrorCode errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}

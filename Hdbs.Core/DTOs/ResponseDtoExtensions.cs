using Hdbs.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Core.DTOs
{
    public static class ResponseDtoExtensions
    {
        public static ResponseDto<T> ToResponseDto<T>(this T data)
        {
            return new ResponseDto<T>
            {
                Data = data,
                Error = null
            };
        }

        public static ResponseDto<T> ToResponseDto<T>(this T data, CustomErrorCode errorCode, string errorMessage)
        {
            return new ResponseDto<T>
            {
                Data = data,
                Error = new ErrorDto
                {
                    ErrorCode = errorCode,
                    ErrorMessage = errorMessage
                }
            };
        }
    }
}

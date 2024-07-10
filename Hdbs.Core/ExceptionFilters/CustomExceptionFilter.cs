using Hdbs.Core.CustomExceptions;
using Hdbs.Core.DTOs;
using Hdbs.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Hdbs.Core.ExceptionFilters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(CustomException))
            {
                if (context.Exception is CustomException ce)
                {
                    _logger?.LogError($"Received CustomException, sending bad request.\n Code:{ce.ErrorCode}\n Messages:{ce.Message}.\n Stack:{ce.StackTrace}.");

                    context.Result = new BadRequestObjectResult(new ResponseDto<object>
                    {
                        Error = new ErrorDto()
                        {
                            ErrorCode = ce.ErrorCode,
                            ErrorMessage = ce.Message
                        }
                    });
                }
            }
            else
            {
                _logger?.LogError($"Received unknown exception, sending bad request.\n Message:{context.Exception.Message}.\n Stack:{context.Exception.StackTrace}.");

                context.Result = new BadRequestObjectResult(new ResponseDto<object>
                {
                    Error = new ErrorDto()
                    {
                        ErrorCode = CustomErrorCode.UnexpectedError,
                        ErrorMessage = CustomErrorCode.UnexpectedError.ToString(),
                    }
                });
            }
        }
    }
}

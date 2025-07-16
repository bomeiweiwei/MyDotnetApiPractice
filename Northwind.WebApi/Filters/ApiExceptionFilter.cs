using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Northwind.Models.CustExceptions;
using Northwind.Utilities.Enum;
using Northwind.WebApi.Models.CustResp;

namespace Northwind.WebApi.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            ErrorResponse error;
            int statusCode;

            if (context.Exception is HttpStatusException httpEx)
            {
                statusCode = httpEx.HttpStatusCode;
                error = new ErrorResponse
                {
                    StatusCode = httpEx.AppStatusCode,
                    Message = httpEx.Message
                };
            }
            else
            {
                statusCode = StatusCodes.Status500InternalServerError;
                error = new ErrorResponse
                {
                    StatusCode = ReturnCode.ExceptionError,
                    Message = "An unexpected error occurred."
                };

                _logger.LogError(context.Exception, "Unhandled exception in API");
            }

            context.Result = new ObjectResult(error)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}


using System;
using Northwind.WebApi.Middlewares;

namespace Northwind.WebApi.Extensions
{
    public static class ApiLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiLoggingMiddleware>();
        }
    }
}


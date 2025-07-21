using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Supplier.Api.Filters
{
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string HeaderName = "X-Internal-ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var validKey = config["InternalApi:ApiKey"];

            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var actualKey) || actualKey != validKey)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}


using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Supplier.Api.Filters
{
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string HeaderName = "Api-Key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var validKey = config["System:ApiKey"];

            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var actualKey) || actualKey != validKey)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}


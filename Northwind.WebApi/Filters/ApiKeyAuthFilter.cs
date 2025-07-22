using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Northwind.Utilities.ConfigManager;
using Northwind.WebApi.Models.CustResp;

namespace Northwind.WebApi.Filters
{
	public class ApiKeyAuthFilter : TypeFilterAttribute
    {
        public ApiKeyAuthFilter() : base(typeof(ApiKeyAuthFilterImpl))
        {
        }
    }

    public class ApiKeyAuthFilterImpl : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 預設未授權
            bool isAuthorized = false;
            int statusCode = StatusCodes.Status401Unauthorized;
            string statusMessage = "未授權：缺少或錯誤的 API 金鑰";

            var request = context.HttpContext.Request;
            var headers = request.Headers;
            string headerName = ConfigManager.SystemSection.HeaderName;
            // 嘗試從指定 Header 取得 API Key
            if (headers.TryGetValue(headerName, out var apiKey))
            {
                if (apiKey == ConfigManager.SystemSection.ApiKey)
                {
                    isAuthorized = true;
                }
            }

            if (!isAuthorized)
            {
                context.Result = new ObjectResult(new CustomErrorResponse(statusMessage, statusCode))
                {
                    StatusCode = statusCode
                };
                return;
            }

            await next();
        }
    }
}


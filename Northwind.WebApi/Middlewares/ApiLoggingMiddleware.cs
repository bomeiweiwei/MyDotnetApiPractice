using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Northwind.WebApi.Middlewares
{
	public class ApiLoggingMiddleware
	{
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiLoggingMiddleware> _logger;

        public ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            string requestBody = string.Empty;
            string responseBody = string.Empty;
            string methodName = "Unknown";

            // 捕獲原始的回應 Stream
            var originalBodyStream = context.Response.Body;
            // 使用 try-finally 確保原始流總是恢復
            using var responseBodyStream = new MemoryStream(); // 在 using 語句中聲明，確保 Dispose

            try
            {
                // 將回應 Stream 替換為我們的 MemoryStream
                context.Response.Body = responseBodyStream;

                if (context.Request.ContentLength > 0 && context.Request.ContentType != null &&
                    (context.Request.ContentType.Contains("application/json") ||
                     context.Request.ContentType.Contains("application/xml") ||
                     context.Request.ContentType.Contains("text/plain") ||
                     context.Request.ContentType.Contains("application/x-www-form-urlencoded")))
                {
                    context.Request.EnableBuffering();
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                    {
                        requestBody = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;
                    }
                }

                var endpoint = context.GetEndpoint();
                if (endpoint != null)
                {
                    var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                    if (controllerActionDescriptor != null)
                    {
                        methodName = $"{controllerActionDescriptor.ControllerName}.{controllerActionDescriptor.ActionName}";
                    }
                }

                // 執行管道中的下一個中介軟體和 API 邏輯
                await _next(context); // 如果這裡拋出例外，下面的讀取回應和複製將不會執行

                // 如果沒有例外發生，則讀取回應內容
                if (context.Response.Body.CanRead && context.Response.ContentType != null &&
                    (context.Response.ContentType.Contains("application/json") ||
                     context.Response.ContentType.Contains("application/xml") ||
                     context.Response.ContentType.Contains("text/plain")))
                {
                    responseBodyStream.Position = 0;
                    using (var reader = new StreamReader(responseBodyStream, Encoding.UTF8, leaveOpen: true))
                    {
                        responseBody = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex) // 這裡捕獲的是 ApiLoggingMiddleware 自身發生的例外，而不是下游拋出的例外
            {
                _logger.LogError(ex, "An error occurred in ApiLoggingMiddleware during request/response processing for {RequestPath}.", context.Request.Path);
                throw; // 重新拋出例外，讓 UseExceptionHandler 捕獲
            }
            finally
            {
                // 確保原始回應流總是恢復，並且只有在 responseBodyStream 有內容時才複製
                // 在發生例外時，responseBodyStream 通常是空的，
                // 並且 UseExceptionHandler 會直接寫入 originalBodyStream
                context.Response.Body = originalBodyStream; // 恢復原始流

                // 如果有內容，則複製（這通常發生在非錯誤情況下）
                if (responseBodyStream.Length > 0)
                {
                    responseBodyStream.Position = 0;
                    await responseBodyStream.CopyToAsync(originalBodyStream);
                }

                if (string.IsNullOrEmpty(responseBody) && context.Items.ContainsKey("ExceptionResponse"))
                {
                    responseBody = context.Items["ExceptionResponse"]?.ToString() ?? "";
                }

                stopwatch.Stop();
                var duration = stopwatch.ElapsedMilliseconds;

                object parsedRequestBody = requestBody;
                object parsedResponseBody = responseBody;

                var logLevel = LogLevel.Information;
                if (context.Response.StatusCode >= 400)
                {
                    logLevel = LogLevel.Warning;
                }
                if (context.Response.StatusCode >= 500)
                {
                    logLevel = LogLevel.Error;
                }

                var logMessage = new
                {
                    //ApiMethod = methodName, // WeatherForecast.Get
                    RequestPath = context.Request.Path.ToString(), // /WeatherForecast
                    QueryString = context.Request.QueryString.ToString(), // ?start=1&end=5
                    RequestMethod = context.Request.Method, // GET
                    //RequestHeaders = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    RequestBody = parsedRequestBody,
                    ResponseStatusCode = context.Response.StatusCode,
                    //ResponseHeaders = context.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    ResponseBody = parsedResponseBody, // 在錯誤情況下，這可能是空的
                    DurationMilliseconds = duration
                };

                //_logger.Log(logLevel, "{@ApiCallLog}", logMessage);

                _logger.Log(logLevel,
                    "RequestPath: {RequestPath}, RequestMethod: {RequestMethod}, QueryString: {QueryString}, RequestBody: {RequestBody}, ResponseStatusCode: {ResponseStatusCode}, ResponseBody: {ResponseBody}, DurationMilliseconds: {DurationMilliseconds}",
                    logMessage.RequestPath,
                    logMessage.RequestMethod,
                    logMessage.QueryString,
                    MinifyJson(logMessage.RequestBody),
                    logMessage.ResponseStatusCode,
                    MinifyJson(logMessage.ResponseBody),
                    logMessage.DurationMilliseconds);
            }
        }

        // 簡單判斷是否為 JSON 格式 (已考慮 null 或空字符串)
        private bool IsJson(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            str = str.Trim();
            return (str.StartsWith("{") && str.EndsWith("}")) || (str.StartsWith("[") && str.EndsWith("]"));
        }
        private string MinifyJson(object obj)
        {
            if (obj == null) return string.Empty;
            try
            {
                var parsed = JsonConvert.DeserializeObject(obj.ToString());
                return JsonConvert.SerializeObject(parsed, Formatting.None);
            }
            catch
            {
                return obj.ToString(); // 無法轉成 JSON 的話就原樣回傳
            }
        }
    }
}


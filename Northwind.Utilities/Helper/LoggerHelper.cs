using System;
using Microsoft.Extensions.Logging;

namespace Northwind.Utilities.Helper
{
    public interface IGenericLogger
    {
        void Info(string message);
        void Debug(string message);
        void Error(string message);
        void Error(Exception ex, string message);
        void HttpClientInfo(string message, string request = "", string respons = "", long duration = 0);
        void Trace(string message, string application = "Api", string method = "", string request = "", string respons = "", long duration = 0);
    }

    public class GenericLogger : IGenericLogger
    {
        private readonly ILogger<GenericLogger> _logger; // 注入 ILogger

        // 構造函數，注入 ILogger
        public GenericLogger(ILogger<GenericLogger> logger)
        {
            _logger = logger;
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }

        public void Debug(string message)
        {
            _logger.LogDebug(message);
        }

        public void Error(string message)
        {
            _logger.LogError(message);
        }

        public void Error(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }

        // 記錄 HttpClient 呼叫資訊，以結構化日誌呈現
        public void HttpClientInfo(string message, string request = "", string respons = "", long duration = 0)
        {
            _logger.LogInformation(
                "HttpClientInfo: {Message}. Request: {HttpRequest}. Response: {HttpResponse}. Duration: {DurationMs}ms",
                message,
                request,
                respons,
                duration
            );
        }

        // 記錄詳細追蹤資訊，包含多個自定義屬性
        public void Trace(string message, string application = "Api", string method = "", string request = "", string respons = "", long duration = 0)
        {
            _logger.LogTrace(
                "Trace: {Message}. Application: {ApplicationName}. Method: {MethodName}. Request: {HttpRequest}. Response: {HttpResponse}. Duration: {DurationMs}ms",
                message,
                application,
                method,
                request,
                respons,
                duration
            );
        }
    }
}


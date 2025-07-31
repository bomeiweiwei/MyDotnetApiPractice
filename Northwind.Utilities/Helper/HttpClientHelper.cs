using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Northwind.Utilities.Enum;
using Northwind.Utilities.Extensions;

namespace Northwind.Utilities.Helper
{

    public interface IHttpClientHelper
    {
        // GET
        Task<string> GetAsync(string url, Dictionary<string, string> headers = null);
        Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null);

        // POST JSON
        Task<string> PostJsonAsync(string url, string jsonString, Dictionary<string, string> headers = null);
        Task<T> PostJsonAsync<T>(string url, string jsonString, Dictionary<string, string> headers = null);

        // POST Form
        Task<string> PostFormAsync(string url, Dictionary<string, string> formData = null, Dictionary<string, string> headers = null);
        Task<T> PostFormAsync<T>(string url, Dictionary<string, string> formData = null, Dictionary<string, string> headers = null);

        // POST File
        Task<T> PostFileAsync<T>(string url, IFormFile file, string fileParamName = "File",
                                   Dictionary<string, string> additionalFormData = null,
                                   Dictionary<string, string> headers = null);
        Task<T> PostFilesAsync<T>(string url, List<IFormFile> files, string fileParamName = "Files",
                                    Dictionary<string, string> additionalFormData = null,
                                    Dictionary<string, string> headers = null);
    }

    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly IGenericLogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientHelper(IGenericLogger logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetAsync(string url, Dictionary<string, string> headers = null)
        {
            var stopwatch = Stopwatch.StartNew(); // 開始計時
            string responseContent = string.Empty;
            string method = HttpMethodCode.GET.GetDescription();

            try
            {
                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // 加入 headers
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                var response = await client.SendAsync(request);
                responseContent = await response.Content.ReadAsStringAsync();

                stopwatch.Stop(); // 停止計時

                if (response.IsSuccessStatusCode)
                {
                    LogInfo(url, method, null, responseContent, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    _logger.Error($"[HttpClient-{method}] 呼叫失敗：{url}, 狀態碼：{response.StatusCode}, 回應：{responseContent}");
                }

                return responseContent;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Error(ex, $"[HttpClient-{method}] 呼叫異常：{url}, 耗時：{stopwatch.ElapsedMilliseconds} ms");
                return null;
            }
        }

        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            var stopwatch = Stopwatch.StartNew(); // 開始計時
            string responseContent = string.Empty;
            string method = HttpMethodCode.GET.GetDescription();

            try
            {
                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // 加入 headers
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                var response = await client.SendAsync(request);
                responseContent = await response.Content.ReadAsStringAsync();

                stopwatch.Stop(); // 停止計時

                if (response.IsSuccessStatusCode)
                {
                    LogInfo(url, method, null, responseContent, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    _logger.Error($"[HttpClient-{method}] 呼叫失敗：{url}, 狀態碼：{response.StatusCode}, 回應：{responseContent}");
                }

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"[HttpClient-{method}] 反序列化失敗，內容：{responseContent}");
                        return default(T); // 反序列化錯誤也回傳 default
                    }
                }
                else
                {
                    return default(T);
                }

            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Error(ex, $"[HttpClient-{method}] 呼叫異常：{url}, 耗時：{stopwatch.ElapsedMilliseconds} ms");
                return default(T);
            }
        }

        private async Task<string> PostAsync(string url, string jsonString, string mediaType = "application/json", Dictionary<string, string> headers = null, Dictionary<string, string> formData = null)
        {
            var stopwatch = Stopwatch.StartNew();
            string responseContent = string.Empty;
            string method = HttpMethodCode.POST.GetDescription();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);

                // 加入 headers
                if (headers != null)
                {
                    foreach (var header in headers)
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                if (formData != null)
                {
                    var contentList = new List<KeyValuePair<string, string>>();
                    foreach (var item in formData)
                        contentList.Add(new KeyValuePair<string, string>(item.Key, item.Value));

                    request.Content = new FormUrlEncodedContent(contentList);
                }
                else
                {
                    request.Content = new StringContent(jsonString ?? "", Encoding.UTF8, mediaType);
                }

                var response = await client.SendAsync(request);
                responseContent = await response.Content.ReadAsStringAsync();

                stopwatch.Stop();

                // Log
                string requestLog = formData != null
                    ? JsonConvert.SerializeObject(formData)
                    : jsonString;

                if (response.IsSuccessStatusCode)
                {
                    LogInfo(url, method, requestLog, responseContent, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    _logger.Error($"[HttpClient-{method}] 呼叫失敗：{url}, 狀態碼：{response.StatusCode}, 回應：{responseContent}");
                }

                return responseContent;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Error(ex, $"[HttpClient-{method}] 呼叫異常：{url}, 耗時：{stopwatch.ElapsedMilliseconds} ms");
                return null;
            }
        }

        private async Task<T> PostAsync<T>(string url, string jsonString, string mediaType = "application/json", Dictionary<string, string> headers = null, Dictionary<string, string> formData = null)
        {
            var stopwatch = Stopwatch.StartNew();
            string responseContent = string.Empty;
            string method = HttpMethodCode.POST.GetDescription();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);

                // 加入 headers
                if (headers != null)
                {
                    foreach (var header in headers)
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                string requestLog;

                if (formData != null)
                {
                    var contentList = new List<KeyValuePair<string, string>>();
                    foreach (var item in formData)
                        contentList.Add(new KeyValuePair<string, string>(item.Key, item.Value));

                    request.Content = new FormUrlEncodedContent(contentList);
                    requestLog = JsonConvert.SerializeObject(formData);
                }
                else
                {
                    request.Content = new StringContent(jsonString ?? "", Encoding.UTF8, mediaType);
                    requestLog = jsonString;
                }

                var response = await client.SendAsync(request);
                responseContent = await response.Content.ReadAsStringAsync();

                stopwatch.Stop();

                if (response.IsSuccessStatusCode)
                {
                    LogInfo(url, method, requestLog, responseContent, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    _logger.Error($"[HttpClient-{method}] 呼叫失敗：{url}, 狀態碼：{response.StatusCode}, 回應：{responseContent}");
                }

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"[HttpClient-{method}] 反序列化失敗，內容：{responseContent}");
                    }
                }

                return default(T);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Error(ex, $"[HttpClient-{method}] 呼叫異常：{url}, 耗時：{stopwatch.ElapsedMilliseconds} ms");
                return default(T);
            }
        }

        public async Task<string> PostJsonAsync(string url, string jsonString, Dictionary<string, string> headers = null)
        {
            return await PostAsync(url, jsonString, MediaTypeCode.Json.ToValue(), headers, null);
        }

        public async Task<T> PostJsonAsync<T>(string url, string jsonString, Dictionary<string, string> headers = null)
        {
            return await PostAsync<T>(url, jsonString, MediaTypeCode.Json.ToValue(), headers, null);
        }

        public async Task<string> PostFormAsync(string url, Dictionary<string, string> formData = null, Dictionary<string, string> headers = null)
        {
            return await PostAsync(url, null, MediaTypeCode.FormUrlEncoded.ToValue(), headers, formData);
        }

        public async Task<T> PostFormAsync<T>(string url, Dictionary<string, string> formData = null, Dictionary<string, string> headers = null)
        {
            return await PostAsync<T>(url, null, MediaTypeCode.FormUrlEncoded.ToValue(), headers, formData);
        }

        public async Task<T> PostFileAsync<T>(string url, IFormFile file, string fileParamName = "File",
                                              Dictionary<string, string> additionalFormData = null,
                                              Dictionary<string, string> headers = null)
        {
            return await PostMultipartAsync<T>(url, new List<IFormFile> { file }, fileParamName, additionalFormData, headers);
        }

        public async Task<T> PostFilesAsync<T>(string url, List<IFormFile> files, string fileParamName = "Files",
                                               Dictionary<string, string> additionalFormData = null,
                                               Dictionary<string, string> headers = null)
        {
            return await PostMultipartAsync<T>(url, files, fileParamName, additionalFormData, headers);
        }

        private async Task<T> PostMultipartAsync<T>(string url, List<IFormFile> files, string fileParamName, Dictionary<string, string> formData = null, Dictionary<string, string> headers = null)
        {
            var stopwatch = Stopwatch.StartNew();
            string responseContent = string.Empty;
            string method = HttpMethodCode.POST.GetDescription();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);

                // 加入 headers
                if (headers != null)
                {
                    foreach (var header in headers)
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                var multipartContent = new MultipartFormDataContent();

                // 加入檔案
                foreach (var file in files)
                {
                    if (file?.Length > 0)
                    {
                        using var stream = file.OpenReadStream();
                        var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        var fileContent = new StreamContent(memoryStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
                        multipartContent.Add(fileContent, fileParamName, file.FileName);
                    }
                }

                // 加入其他 Form 欄位
                if (formData != null)
                {
                    foreach (var kvp in formData)
                    {
                        multipartContent.Add(new StringContent(kvp.Value), kvp.Key);
                    }
                }

                request.Content = multipartContent;

                var response = await client.SendAsync(request);
                responseContent = await response.Content.ReadAsStringAsync();

                stopwatch.Stop();

                // 組 request log (只記 form 內容，不記 raw 檔案)
                string requestLog = formData != null ? JsonConvert.SerializeObject(formData) : "(Only File Upload)";

                if (response.IsSuccessStatusCode)
                {
                    LogInfo(url, method, requestLog, responseContent, stopwatch.ElapsedMilliseconds);
                }
                else
                {
                    _logger.Error($"[HttpClient-{method}] 呼叫失敗：{url}, 狀態碼：{response.StatusCode}, 回應：{responseContent}");
                }

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"[HttpClient-{method}] 檔案回傳反序列化失敗，內容：{responseContent}");
                    }
                }

                return default(T);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.Error(ex, $"[HttpClient-{method}] 檔案上傳呼叫異常：{url}, 耗時：{stopwatch.ElapsedMilliseconds} ms");
                return default(T);
            }
        }

        private void LogInfo(string url, string method, string requestData, string responseData, long elapsedTime)
        {
            _logger.HttpClientInfo($"[HttpClient-{method}] 呼叫成功：{url}", requestData ?? "", responseData ?? "", elapsedTime);
        }
    }
}
using System;
using System.Text;
using System.Text.Json;

namespace Supplier.Api.Helper
{
    public static class ApiCallerHelper
    {
        public static async Task<T> GetAsync<T>(HttpClient client, string url, string apiKey, string headerName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add(headerName, apiKey);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public static async Task<TResponse> PostAsync<TRequest, TResponse>(
            HttpClient client,
            string url,
            TRequest data,
            string apiKey,
            string headerName)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add(headerName, apiKey);
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(result);
        }
    }

}


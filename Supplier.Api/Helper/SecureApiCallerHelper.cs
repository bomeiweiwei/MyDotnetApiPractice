using System;
using System.Text;
using System.Text.Json;

namespace Supplier.Api.Helper
{
    public static class SecureApiCallerHelper
    {
        public static async Task<string> PostEncryptedStringAsync(
            HttpClient client,
            string url,
            string input,
            string apiKey,
            string headerName,
            string aesKeyBase64,
            string aesIVBase64)
        {
            var payload = new { EncryptedData = input };
            var payloadJson = JsonSerializer.Serialize(payload);

            var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add(headerName, apiKey);
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var encryptedResult = await response.Content.ReadAsStringAsync();
            return AesEncryptionHelper.Decrypt(encryptedResult, aesKeyBase64, aesIVBase64);
        }
    }
}


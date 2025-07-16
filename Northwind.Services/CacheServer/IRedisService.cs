using System;
namespace Northwind.Services.CacheServer
{
	public interface IRedisService
	{
        Task SetStringAsync(string key, string value, TimeSpan? expiry = null);
        Task<string?> GetStringAsync(string key);
        Task<bool> KeyExistsAsync(string key);
        Task<bool> DeleteKeyAsync(string key);
    }
}


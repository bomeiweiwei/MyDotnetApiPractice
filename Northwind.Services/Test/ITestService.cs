using System;
using Northwind.Models;

namespace Northwind.Services.Test
{
	public interface ITestService
	{
        Task<ApiResponseBase<bool>> GetConnectResult();

        Task<ApiResponseBase<bool>> RedisSetValue();

        Task<ApiResponseBase<string>> RedisGetValue();
    }
}


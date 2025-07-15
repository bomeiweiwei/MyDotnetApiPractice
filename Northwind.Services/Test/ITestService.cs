using System;
using Northwind.Models;

namespace Northwind.Services.Test
{
	public interface ITestService
	{
        Task<ApiResponseBase<bool>> GetConnectResult();
    }
}


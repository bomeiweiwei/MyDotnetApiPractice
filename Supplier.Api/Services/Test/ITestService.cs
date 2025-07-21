using System;
using Supplier.Api.Models;
using Supplier.Api.Models.Api;
using Supplier.Api.Models.Config.External;

namespace Supplier.Api.Services.Test
{
	public interface ITestService
	{
        Task<ApiResponseBase<OptionsGetValueResp>> OptionsGetValue();
    }
}


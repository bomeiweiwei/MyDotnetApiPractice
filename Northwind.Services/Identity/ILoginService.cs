using System;
using Northwind.Models;
using Northwind.Models.Identity;

namespace Northwind.Services.Identity
{
	public interface ILoginService
	{
        Task<ApiResponseBase<LoginResp>> Login(LoginReq req);
    }
}


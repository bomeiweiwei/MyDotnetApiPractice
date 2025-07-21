using System;
using Supplier.Api.Models;
using Supplier.Api.Models.Api;
using Supplier.Api.Models.Identity;

namespace Supplier.Api.Services.Identity
{
    public interface ILoginService
    {
        Task<ApiResponseBase<LoginResp>> Login(LoginReq req);
    }
}


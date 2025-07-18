using System;
using Supplier.Api.Models;

namespace Supplier.Api.Services.Identity
{
    public interface ILoginService
    {
        Task<ApiResponseBase<LoginResp>> Login(LoginReq req);
    }
}


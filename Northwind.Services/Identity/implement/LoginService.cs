using System;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Northwind.Models;
using Northwind.Models.FakeDatas;
using Northwind.Models.Identity;
using Northwind.Utilities.ConfigManager;
using Northwind.Utilities.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Northwind.Utilities.Enum;
using Northwind.Models.CustExceptions;

namespace Northwind.Services.Identity.implement
{
    public class LoginService : BaseService, ILoginService
    {
        public LoginService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        private List<FakeAccount> FakeAccounts = new List<FakeAccount>()
        {
            new FakeAccount()
            {
                AccountId = 1,
                UserId = "Admin",
                Password = "1234",
                UserName = "系統管理員",
                // 假裝是DB讀到的權限數字
                Permissions = new List<int>()
                {
                    (int)PermissionCode.ViewEmployees,
                    (int)PermissionCode.ViewCategories,
                    (int)PermissionCode.CreateCategory
                }
            },
            new FakeAccount()
            {
                AccountId = 2,
                UserId = "Employee1",
                Password = "1234",
                UserName = "員工1號",
                // 假裝是DB讀到的權限數字
                Permissions = new List<int>()
                {
                    (int)PermissionCode.ViewCategories
                }
            },
            new FakeAccount()
            {
                AccountId = 3,
                UserId = "Member1",
                Password = "1234",
                UserName = "會員1號",
                Permissions = new List<int>()
            }
        };


        public async Task<ApiResponseBase<LoginResp>> Login(LoginReq req)
        {
            var result = new ApiResponseBase<LoginResp>()
            {
                Data = new LoginResp()
            };
            var query = FakeAccounts.Where(m => m.UserId == req.UserId && m.Password == req.Password).FirstOrDefault();
            if (query != null)
            {
                DateTime expirationTime = DateTime.UtcNow.AddMinutes(15);
                TimeSpan ttl = expirationTime - DateTime.UtcNow;
                JwtUserInfo userInfo = new JwtUserInfo()
                {
                    AccountId = query.AccountId,
                    UserName = query.UserName,
                    Permissions = query.Permissions,
                    Expiration = expirationTime
                };
                var token = GenerateJwtToken(userInfo);

                //存redis
                await base.RedisService().SetStringAsync($"Login:{query.AccountId}", token, ttl);

                result.Data = new LoginResp
                {
                    JwtToken = token
                };
            }
            else
            {
                throw new UnauthorizedException("驗證失敗");
            }

            return result;
        }

        private string GenerateJwtToken(JwtUserInfo userInfo)
        {
            var claims = new List<Claim>
            {
                new Claim("AccountId", userInfo.AccountId.ToString()),
                new Claim("UserName", userInfo.UserName),
                new Claim("Expiration", userInfo.Expiration.ToString("o"))
            };

            // 多個 Permission 權限
            foreach (var permission in userInfo.Permissions)
            {
                claims.Add(new Claim("Permission", permission.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigManager.JwtSection.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: ConfigManager.JwtSection.Issuer,
                audience: ConfigManager.JwtSection.Audience,
                claims: claims,
                expires: userInfo.Expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


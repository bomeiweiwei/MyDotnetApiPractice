using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Supplier.Api.Models;

namespace Supplier.Api.Services.Identity.implement
{
	public class LoginService: ILoginService
    {
        private readonly IConfiguration _configuration;

        public LoginService(IConfiguration configuration)
		{
            _configuration = configuration;

        }

        private List<FakeAccount> FakeAccounts = new List<FakeAccount>()
        {
            new FakeAccount()
            {
                AccountId = 1,
                UserId = "Admin",
                Password = "1234",
                UserName = "系統管理員"
            },
            new FakeAccount()
            {
                AccountId = 2,
                UserId = "Employee1",
                Password = "1234",
                UserName = "員工1號"
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
                DateTime expirationTime = DateTime.UtcNow.AddHours(1);
                TimeSpan ttl = expirationTime - DateTime.UtcNow;
                JwtUserInfo userInfo = new JwtUserInfo()
                {
                    AccountId = query.AccountId,
                    UserName = query.UserName
                };
                var token = GenerateJwtToken(userInfo);

                result.Data = new LoginResp
                {
                    JwtToken = token
                };
            }

            return result;
        }

        private string GenerateJwtToken(JwtUserInfo user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


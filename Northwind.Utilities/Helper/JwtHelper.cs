using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Northwind.Utilities.Helper
{
    public static class JwtHelper
    {
        public static ClaimsPrincipal? ValidateToken(string token, IConfiguration configuration, out JwtSecurityToken? jwtToken, out string? error)
        {
            jwtToken = null;
            error = null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = false, // 手動驗證過期時間
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwtToken = validatedToken as JwtSecurityToken;
                return principal;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
    }
}


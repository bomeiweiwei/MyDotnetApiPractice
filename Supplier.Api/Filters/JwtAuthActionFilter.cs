using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Supplier.Api.Models;
using Supplier.Api.Models.CustResp;

namespace Supplier.Api.Filters
{
    public class JwtAuthActionFilter : TypeFilterAttribute
    {
        public JwtAuthActionFilter() : base(typeof(JwtAuthActionFilterImpl))
        {
        }
    }

    public class JwtAuthActionFilterImpl : IAsyncActionFilter
    {
        private readonly IConfiguration _configuration;
        public JwtAuthActionFilterImpl(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(new CustomErrorResponse("Missing token"))
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // 可選：將驗證通過的 user 資訊加入 HttpContext.User
                context.HttpContext.User = principal;

                await next(); // 繼續執行後續的 middleware 或 action
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new JsonResult(new CustomErrorResponse("Token expired", StatusCodes.Status403Forbidden));
            }
            catch (Exception)
            {
                context.Result = new JsonResult(new CustomErrorResponse("Invalid token", StatusCodes.Status403Forbidden));
            }

        }
    }
}


using System;
using Northwind.Models.Identity;
using System.Security.Claims;

namespace Northwind.Services.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static JwtUserInfo ToJwtUserInfo(this ClaimsPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            List<int> permissionList = user.Claims
                                        .Where(c => c.Type == "Permission")
                                        .Select(c => int.Parse(c.Value))
                                        .ToList();

            return new JwtUserInfo
            {
                AccountId = int.TryParse(user.FindFirst("AccountId")?.Value, out var accountId) ? accountId : 0,
                UserName = user.FindFirst("UserName")?.Value,
                Permissions = permissionList.Any() ? permissionList: new List<int>(),
                Expiration = DateTime.TryParse(user.FindFirst("Expiration")?.Value, out var expiration) ? expiration : DateTime.MinValue
            };
        }

        public static List<string> GetPermissions(this ClaimsPrincipal user)
        {
            return user?.Claims
                       .Where(c => c.Type == "Permission")
                       .Select(c => c.Value)
                       .ToList() ?? new List<string>();
        }
    }
}


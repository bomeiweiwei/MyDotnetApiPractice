using System;
namespace Supplier.Api.Models.Identity
{
    public class JwtUserInfo
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public DateTime Expiration { get; set; }
    }
}


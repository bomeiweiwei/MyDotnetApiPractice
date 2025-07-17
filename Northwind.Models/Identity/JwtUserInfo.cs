using System;

namespace Northwind.Models.Identity
{
	public class JwtUserInfo
	{
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public List<int> Permissions { get; set; }
        public DateTime Expiration { get; set; }
    }
}


using System;
namespace Supplier.Api.Models
{
	public class FakeAccount
	{
        /// <summary>
        /// 帳號流水號
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }
    }
}


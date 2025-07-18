using System;
namespace Supplier.Api.Models
{
	public class ApiResponseBase<T>
    {
        /// <summary>
        /// 訊息列表
        /// </summary>
        public string Message { get; set; } = "成功";
        /// <summary>
        /// 內容
        /// </summary>
        public T Data { get; set; }
    }
}


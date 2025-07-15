using System;
using Northwind.Utilities.Enum;

namespace Northwind.Models
{
    public class ApiResponseBase<T>
    {
        /// <summary>
        /// 狀態代碼
        /// </summary>
        public long StatusCode { get; set; } = (int)ReturnCode.Succeeded;
        /// <summary>
        /// 訊息列表
        /// </summary>
        public string Message { get; set; } = "成功";
        /// <summary>
        /// 內容
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 總筆數
        /// </summary>
        public long Count { get; set; } = 0;
    }
}


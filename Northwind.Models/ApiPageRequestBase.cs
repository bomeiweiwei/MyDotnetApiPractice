using System;
namespace Northwind.Models
{
    public class ApiPageRequestBase<T>
    {
        /// <summary>
        /// 頁碼，預設第1頁
        /// </summary>
        public int pageIndex { get; set; } = 1;
        /// <summary>
        /// 筆數，預設10筆
        /// </summary>
        public int pageSize { get; set; } = 10;
        /// <summary>
        /// 內容
        /// </summary>
        public T Data { get; set; } = default(T);
    }
}


using System;
using Northwind.Utilities.Enum;

namespace Northwind.WebApi.Models.CustResp
{
    public class ErrorResponse
    {
        public ReturnCode StatusCode { get; set; }
        public string Message { get; set; } // 錯誤訊息
    }
}


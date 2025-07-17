using System;
using Northwind.Utilities.Enum;

namespace Northwind.WebApi.Models.CustResp
{
    public class CustomErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public CustomErrorResponse(string message, int statusCode = StatusCodes.Status401Unauthorized)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}


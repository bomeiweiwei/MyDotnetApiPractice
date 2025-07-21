using System;
namespace Supplier.Api.Models.CustResp
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


using System;
using Microsoft.AspNetCore.Http;
using Northwind.Utilities.Enum;

namespace Northwind.Utilities.CustExceptions
{
    public class BusinessException : HttpStatusException
    {
        public BusinessException(ReturnCode appCode, string message)
            : base(StatusCodes.Status400BadRequest, appCode, message)
        {
        }
    }
}


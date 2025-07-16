using System;
using Microsoft.AspNetCore.Http;
using Northwind.Utilities.Enum;

namespace Northwind.Utilities.CustExceptions
{
    public class UnauthorizedException : HttpStatusException
    {
        public UnauthorizedException(string message = "Unauthorized")
            : base(StatusCodes.Status401Unauthorized, ReturnCode.Unauthorized, message)
        {
        }
    }
}


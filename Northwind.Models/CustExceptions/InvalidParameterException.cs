using System;
using Microsoft.AspNetCore.Http;
using Northwind.Utilities.Enum;

namespace Northwind.Models.CustExceptions
{
    public class InvalidParameterException : HttpStatusException
    {
        public InvalidParameterException(string message)
            : base(StatusCodes.Status400BadRequest, ReturnCode.InvalidParameter, message)
        {
        }
    }
}


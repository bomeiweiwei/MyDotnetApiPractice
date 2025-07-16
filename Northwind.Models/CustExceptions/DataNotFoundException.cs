using System;
using Microsoft.AspNetCore.Http;
using Northwind.Utilities.Enum;

namespace Northwind.Models.CustExceptions
{
    public class DataNotFoundException : HttpStatusException
    {
        public DataNotFoundException(string message)
            : base(StatusCodes.Status404NotFound, ReturnCode.DataNotExisted, message)
        {
        }
    }
}


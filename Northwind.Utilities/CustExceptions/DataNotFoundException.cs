using System;
using Microsoft.AspNetCore.Http;
using Northwind.Utilities.Enum;

namespace Northwind.Utilities.CustExceptions
{
    public class DataNotFoundException : HttpStatusException
    {
        public DataNotFoundException(string message)
            : base(StatusCodes.Status404NotFound, ReturnCode.DataNotExisted, message)
        {
        }
    }
}


using System;
using Northwind.Utilities.Enum;

namespace Northwind.Utilities.CustExceptions
{
    public abstract class HttpStatusException : Exception
    {
        public int HttpStatusCode { get; }

        public ReturnCode AppStatusCode { get; }

        protected HttpStatusException(int httpStatusCode, ReturnCode appStatusCode, string message)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            AppStatusCode = appStatusCode;
        }
    }
}


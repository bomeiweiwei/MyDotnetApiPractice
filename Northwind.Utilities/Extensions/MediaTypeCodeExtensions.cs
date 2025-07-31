using System;
using Northwind.Utilities.Enum;

namespace Northwind.Utilities.Extensions
{
    public static class MediaTypeCodeExtensions
    {
        public static string ToValue(this MediaTypeCode type) => type switch
        {
            MediaTypeCode.Json => "application/json",
            MediaTypeCode.FormUrlEncoded => "application/x-www-form-urlencoded",
            MediaTypeCode.Xml => "application/xml",
            MediaTypeCode.Text => "text/plain",
            _ => "application/json"
        };
    }
}


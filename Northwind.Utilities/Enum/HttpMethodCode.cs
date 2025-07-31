using System;
using System.ComponentModel;

namespace Northwind.Utilities.Enum
{
	public enum HttpMethodCode
	{
        [Description("GET")]
        GET,

        [Description("POST")]
        POST,

        [Description("PUT")]
        PUT,

        [Description("DELETE")]
        DELETE
    }
}


using System;
using System.ComponentModel;

namespace Northwind.Utilities.Enum
{
	public enum ReturnCode
	{
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Succeeded = 0,
        /// <summary>
        /// 異常錯誤
        /// </summary>
        [Description("異常錯誤")]
        ExceptionError = 500
    }
}


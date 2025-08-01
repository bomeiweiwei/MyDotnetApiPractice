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
        Succeeded = 200,
        /// <summary>
        /// 參數錯誤
        /// </summary>
        [Description("參數錯誤")]
        InvalidParameter = 1001,
        /// <summary>
        /// 指定資料不存在
        /// </summary>
        [Description("指定資料不存在")]
        DataNotExisted = 1002,
        /// <summary>
        /// 指定資料存在
        /// </summary>
        [Description("指定資料存在")]
        DataAlreadyExisted = 1003,
        /// <summary>
        /// 未授權
        /// </summary>
        [Description("未授權")]
        Unauthorized = 401,
        /// <summary>
        /// 無權限
        /// </summary>
        [Description("無權限")]
        PermissionDenied = 403,
        /// <summary>
        /// 異常錯誤
        /// </summary>
        [Description("異常錯誤")]
        ExceptionError = 500,

        ExternalApiExceptionError = 1004
    }
}


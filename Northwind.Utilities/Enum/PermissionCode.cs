using System;
using System.ComponentModel;

namespace Northwind.Utilities.Enum
{
	public enum PermissionCode
	{
        [Description("查看員工")]
        ViewEmployees = 1000,

        [Description("管理商品類別")]
        ManageCategories = 2000,
        [Description("查看商品類別")]
        ViewCategories = 2001,
        [Description("新增商品類別")]
        CreateCategory = 2002,
    }
}


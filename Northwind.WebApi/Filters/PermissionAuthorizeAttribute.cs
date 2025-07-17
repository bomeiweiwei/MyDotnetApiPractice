using System;
using Northwind.Utilities.Enum;

namespace Northwind.WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAuthorizeAttribute : Attribute
    {
        public List<int> RequiredPermissions { get; }

        public PermissionAuthorizeAttribute(params PermissionCode[] permissions)
        {
            RequiredPermissions = permissions.Select(p => (int)p).ToList();
        }
    }
}


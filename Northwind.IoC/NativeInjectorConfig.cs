using System;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Northwind.Services;
using Northwind.Utilities.Helper;
using Northwind.Services.CacheServer;
using Northwind.Services.CacheServer.implement;
using Northwind.Utilities.ConfigManager;

namespace Northwind.IoC
{
    public static class NativeInjectorConfig
    {
        /// <summary>
        /// 註冊 DI
        /// </summary>
        /// <param name="service"></param>
        public static void RegisterService(this IServiceCollection service)
        {
            service.AddSingleton<IRedisService>(provider => new RedisService(ConfigManager.ConnectionStrings.Redis));
            service.AddSingleton<IGenericLogger, GenericLogger>();
            service.RegisterInheritedTypes(typeof(BaseService).Assembly, typeof(BaseService));
        }

        /// <summary>
        /// 自動註冊
        /// </summary>
        public static void RegisterInheritedTypes(this IServiceCollection container, Assembly assembly, Type baseType)
        {
            // 取得組件中所有型別
            var allTypes = assembly.GetTypes();
            // 取得基底類別(BaseService)的所有介面
            var baseInterface = baseType.GetInterfaces();

            foreach (var type in allTypes)
            {
                // 檢查是否繼承自 BaseService（包含間接繼承）
                if (!type.IsAbstract && type.IsClass && baseType.IsAssignableFrom(type))
                {
                    // 取得所有該類別實作的介面
                    var interfaces = type.GetInterfaces()
                        .Where(x => !baseInterface.Any(bi => bi.GenericEq(x)));

                    // 註冊每個介面
                    foreach (var typeInterface in interfaces)
                    {
                        container.AddScoped(typeInterface, type);
                    }

                    // 如果沒有找到介面，可以選擇註冊類別本身
                    if (!interfaces.Any())
                    {
                        container.AddScoped(type);
                    }
                }
            }
        }

        public static bool GenericEq(this Type type, Type toCompare)
        {
            // 比較兩個型別的命名空間和名稱是否相同
            return type.Namespace == toCompare.Namespace && type.Name == toCompare.Name;
        }      
    }
}


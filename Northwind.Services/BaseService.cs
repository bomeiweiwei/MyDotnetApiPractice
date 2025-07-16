using System;
using Microsoft.EntityFrameworkCore;
using Northwind.Entities.NorthwindContext.Data;
using Northwind.Utilities.ConfigManager;
using Northwind.Utilities.Enum;
using System.Runtime.InteropServices;
using Northwind.Entities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Services.CacheServer;

namespace Northwind.Services
{
	public class BaseService
	{
		public BaseService()
		{
        }
        
        protected virtual NorthwindContext NorthwindDB([Optional] ConnectionMode connectionMode)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();
            if (connectionMode == ConnectionMode.Master)
            {
                optionsBuilder.OptionsBuilderSetting(ConfigManager.ConnectionStrings.Master);
            }
            else
            {
                optionsBuilder.OptionsBuilderSetting(ConfigManager.ConnectionStrings.Slave);
            }
            return new NorthwindContext(optionsBuilder.Options);
        }
        #region ServiceProvider
        private static IServiceProvider? _serviceProvider;
        public static IServiceProvider ServiceProvider
        {
            get => _serviceProvider ?? throw new InvalidOperationException("ServiceProvider 尚未初始化");
            set
            {
                if (_serviceProvider != null)
                    throw new InvalidOperationException("ServiceProvider 已經初始化，不能再次設置");
                _serviceProvider = value;
            }
        }
        #endregion
        #region 手動實現雙重檢查鎖定（Double-Checked Locking）
        private static IRedisService? _redisService;
        private static readonly object _lock = new object();
        protected virtual IRedisService RedisService()
        {
            if (_redisService == null)
            {
                lock (_lock)
                {
                    if (_redisService == null)
                    {
                        if (ServiceProvider == null)
                            throw new InvalidOperationException("ServiceProvider 尚未初始化");

                        _redisService = ServiceProvider.GetRequiredService<IRedisService>();
                    }
                }
            }
            return _redisService;
        }
        #endregion
    }
}


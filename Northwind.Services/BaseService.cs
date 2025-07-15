using System;
using Microsoft.EntityFrameworkCore;
using Northwind.Entities.NorthwindContext.Data;
using Northwind.Utilities.ConfigManager;
using Northwind.Utilities.Enum;
using System.Runtime.InteropServices;
using Northwind.Entities.Extensions;

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
    }
}


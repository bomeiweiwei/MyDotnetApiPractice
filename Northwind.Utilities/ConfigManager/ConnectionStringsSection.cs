using System;
using Microsoft.Extensions.Configuration;

namespace Northwind.Utilities.ConfigManager
{
	public class ConnectionStringsSection
	{
        private static IConfigurationSection _section;
        public ConnectionStringsSection(IConfigurationSection section)
        {
            _section = section;
        }

        public string Master => _section["MasterConnection"];
        public string Slave => _section["SlaveConnection"];
        public string Redis => _section["RedisConnection"];
    }
}


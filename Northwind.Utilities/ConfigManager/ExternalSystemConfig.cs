using System;
using Microsoft.Extensions.Configuration;

namespace Northwind.Utilities.ConfigManager
{
	public class ExternalSystemConfig
	{
        private readonly IConfigurationSection _section;

        public ExternalSystemConfig(IConfigurationSection section)
        {
            _section = section;
        }

        public string ApiKey => _section["ApiKey"];
        public string HeaderName => _section["HeaderName"];
    }
}


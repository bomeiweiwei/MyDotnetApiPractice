using System;
using Microsoft.Extensions.Configuration;

namespace Northwind.Utilities.ConfigManager
{
	public class JwtSection
	{
        private static IConfigurationSection _section;
        public JwtSection(IConfigurationSection section)
        {
            _section = section;
        }

        public string Key => _section["Key"];
        public string Issuer => _section["Issuer"];
        public string Audience => _section["Audience"];
    }
}


using System;
using Microsoft.Extensions.Configuration;

namespace Northwind.Utilities.ConfigManager
{
	public class ConfigManager
	{
        public static ConnectionStringsSection ConnectionStrings { get; private set; }
        public static JwtSection JwtSection { get; private set; }

        public static void Initial(IConfiguration configuration)
        {
            ConnectionStrings = new ConnectionStringsSection(configuration.GetSection("ConnectionStrings"));
            JwtSection = new JwtSection(configuration.GetSection("Jwt"));
        }
    }
}


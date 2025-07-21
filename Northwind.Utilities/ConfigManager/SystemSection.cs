using System;
using Microsoft.Extensions.Configuration;

namespace Northwind.Utilities.ConfigManager
{
    public class SystemSection
    {
        private static IConfigurationSection _section;
        public SystemSection(IConfigurationSection section)
        {
            _section = section;
        }

        public string ApiKey => _section["ApiKey"];
        public string HeaderName => _section["HeaderName"];
        public List<string> WithOrigins => _section.GetSection("WithOrigins").Get<List<string>>();
    }
}


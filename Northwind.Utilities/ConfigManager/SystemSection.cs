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

        // ApiConfigs 子區塊
        public string ApiKey => _section.GetSection("ApiConfigs")["ApiKey"];
        public string HeaderName => _section.GetSection("ApiConfigs")["HeaderName"];

        // AesConfigs 子區塊
        public string AesKey => _section.GetSection("AesConfigs")["AesKey"];
        public string AesIv => _section.GetSection("AesConfigs")["AesIv"];

        public List<string> WithOrigins => _section.GetSection("WithOrigins").Get<List<string>>();
    }
}


using System;
using Microsoft.Extensions.Configuration;

namespace Northwind.Utilities.ConfigManager
{
	public class ExternalSystemsSection
	{
        private readonly IConfigurationSection _section;

        public ExternalSystemsSection(IConfigurationSection section)
        {
            _section = section;
        }

        public ExternalSystemConfig FakeSupplier => new ExternalSystemConfig(_section.GetSection("FakeSupplier"));
        public ExternalSystemConfig FakeBank1 => new ExternalSystemConfig(_section.GetSection("FakeBank1"));
        public ExternalSystemConfig FakeBank2 => new ExternalSystemConfig(_section.GetSection("FakeBank2"));

        // 可擴充支援動態系統名稱：
        public ExternalSystemConfig GetSystem(string systemName)
        {
            return new ExternalSystemConfig(_section.GetSection(systemName));
        }
    }
}


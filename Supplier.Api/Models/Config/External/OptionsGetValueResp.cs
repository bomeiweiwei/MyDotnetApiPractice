using System;
namespace Supplier.Api.Models.Config.External
{
	public class OptionsGetValueResp
	{
		public List<ExternalSystemSetting> ExternalSystemSettings { get; set; }

    }

	public class ExternalSystemSetting
	{
		public string SysName { get; set; }
		public string Url { get; set; }
        public string ApiKey { get; set; }
		public string Header { get; set; }
    }
}


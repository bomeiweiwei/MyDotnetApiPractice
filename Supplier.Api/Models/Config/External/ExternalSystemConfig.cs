using System;
namespace Supplier.Api.Models.Config.External
{
	public class ExternalSystemConfig
	{
        public string ApiServerUrl { get; set; }
        public ApiConfigs ApiConfigs { get; set; }
        public AesConfigs AesConfigs { get; set; }
    }

    public class ApiConfigs
    {
        public string ApiKey { get; set; }
        public string HeaderName { get; set; }
    }

    public class AesConfigs
    {
        public string AesKey { get; set; }
        public string AesIv { get; set; }
    }
}


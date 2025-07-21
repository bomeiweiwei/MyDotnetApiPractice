using System;
using Microsoft.Extensions.Options;
using Supplier.Api.Models;
using Supplier.Api.Models.Api;
using Supplier.Api.Models.Config.External;
using Supplier.Api.Models.Config.Sys;

namespace Supplier.Api.Services.Test.implement
{
    public class TestService : ITestService
    {
        private readonly SystemSettings _systemSettings;
        private readonly ExternalSystemsOptions _externalSystems;
        public TestService(IOptions<SystemSettings> systemOptions, IOptions<ExternalSystemsOptions> options)
        {
            _systemSettings = systemOptions.Value;
            _externalSystems = options.Value;
        }

        public async Task<ApiResponseBase<OptionsGetValueResp>> OptionsGetValue()
        {
            var result = new ApiResponseBase<OptionsGetValueResp>()
            {
                Data = new OptionsGetValueResp()
                {
                    ExternalSystemSettings = new List<ExternalSystemSetting>()
                }
            };

            string test = _systemSettings.HeaderName;

            foreach (var kv in _externalSystems)
            {
                var setting = new ExternalSystemSetting
                {
                    SysName = kv.Key,
                    ApiKey = kv.Value.ApiKey,
                    Header = kv.Value.HeaderName
                };
                result.Data.ExternalSystemSettings.Add(setting);
            }
            return result;
        }
    }
}


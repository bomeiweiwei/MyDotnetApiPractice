using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Supplier.Api.Helper;
using Supplier.Api.Models;
using Supplier.Api.Models.Api;
using Supplier.Api.Models.Config.External;
using Supplier.Api.Models.Config.Sys;

namespace Supplier.Api.Services.Test.implement
{
    public class TestService : ITestService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SystemSettings _systemSettings;
        private readonly ExternalSystemsOptions _externalSystems;

        public TestService(IHttpClientFactory httpClientFactory, IOptions<SystemSettings> systemOptions, IOptions<ExternalSystemsOptions> options)
        {
            _httpClientFactory = httpClientFactory;
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
                    Url = kv.Value.ApiServerUrl,
                    ApiKey = kv.Value.ApiConfigs.ApiKey,
                    Header = kv.Value.ApiConfigs.HeaderName
                };
                result.Data.ExternalSystemSettings.Add(setting);
            }
            return result;
        }

        public async Task<ApiResponseBase<GetProductResp>> GetProduct(int id)
        {
            var result = new ApiResponseBase<GetProductResp>()
            {
                Data = new GetProductResp()
            };

            var config = _externalSystems["Northwind"];

            var client = _httpClientFactory.CreateClient();
            var url = $"{config.ApiServerUrl}/api/External/Products/GetProduct?id={id}";

            var resp = await ApiCallerHelper.GetAsync<ApiResponseBase<GetProductResp>>(client, url, config.ApiConfigs.ApiKey, config.ApiConfigs.HeaderName);
            if (resp.Data != null && resp.StatusCode == 200)
            {
                result.Data = resp.Data;
            }
            return result;
        }

        public async Task<ApiResponseBase<CustomerDetailData>> GetCustomerDetailData(SensitiveData req)
        {
            var result = new ApiResponseBase<CustomerDetailData>()
            {
                Data = new CustomerDetailData()
            };

            SensitiveData data = new SensitiveData()
            {
                Account = req.Account,
                IdNumber = req.IdNumber
            };
            
            if (_externalSystems.TryGetValue("Northwind", out var config))
            {
                string input = AesEncryptionHelper.EncryptObject(data, config.AesConfigs.AesKey, config.AesConfigs.AesIv);
                var client = _httpClientFactory.CreateClient();
                var url = $"{config.ApiServerUrl}/api/External/Customer/GetCustomerDetailData";

                var resp = await SecureApiCallerHelper.PostEncryptedStringAsync
                    (client, url, input, config.ApiConfigs.ApiKey, config.ApiConfigs.HeaderName, config.AesConfigs.AesKey, config.AesConfigs.AesIv);

                result.Data = JsonSerializer.Deserialize<CustomerDetailData>(resp);
            }
            // 呼叫 API 傳送 secureRequest

            return result;
        }
    }
}


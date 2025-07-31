using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Northwind.Models;
using Northwind.Models.CustExceptions;
using Northwind.Models.External;
using Northwind.Utilities.ConfigManager;
using Northwind.Utilities.Enum;
using Northwind.Utilities.Extensions;
using Northwind.Utilities.Helper;

namespace Northwind.Repository.Supplier.implement
{
	public class SupplierRepository: ISupplierRepository
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IGenericLogger _genericLogger;
        public SupplierRepository(IHttpClientHelper httpClientHelper, IGenericLogger genericLogger)
		{
            _httpClientHelper = httpClientHelper;
            _genericLogger = genericLogger;
        }

        private Dictionary<string, string> GenerateApiHeaders()
        {
            return new Dictionary<string, string>
            {
                { "Api-Key",ConfigManager.ExternalSystemsSection.FakeSupplier.ApiKey }
            };
        }

        public async Task<ApiResponseBase<List<OrderData>>> GetOrderDatas(ApiPageRequestBase<QueryOrderArgs> req)
        {
            var result = new ApiResponseBase<List<OrderData>>()
            {
                Data = new List<OrderData>()
            };

            try
            {
                var headers = GenerateApiHeaders();
                var resp = await _httpClientHelper.PostJsonAsync<ApiResponseBase<List<OrderData>>>(
                            $"{ConfigManager.ExternalSystemsSection.FakeSupplier.ApiServerUrl}/api/Orders/GetOrderDatas",
                            JsonConvert.SerializeObject(req),
                            headers);
                if (resp?.Data != null && resp.StatusCode == (long)ReturnCode.Succeeded)
                {
                    result = resp;
                }
                else
                {
                    throw new BusinessException(ReturnCode.ExternalApiExceptionError, "呼叫 Supplier GetOrderDatas api 失敗");
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _genericLogger.Error("呼叫外部 API 發生未預期錯誤：" + ex.Message);
                throw new BusinessException(ReturnCode.ExceptionError, ReturnCode.ExceptionError.GetDescription());
            }
            
            return result;
        }
    }
}


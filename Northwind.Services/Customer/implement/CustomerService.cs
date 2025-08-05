using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Northwind.Models;
using Northwind.Models.CustExceptions;
using Northwind.Models.External;
using Northwind.Utilities.ConfigManager;
using Northwind.Utilities.Enum;
using Northwind.Utilities.Extensions;
using Northwind.Utilities.Helper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Northwind.Services.Customer.implement
{
    public class CustomerService :BaseService, ICustomerService
    {
        private IGenericLogger _logger;
        public CustomerService(IHttpContextAccessor httpContextAccessor, IGenericLogger logger) : base(httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<string> GetCustomerDetailData(SecureRequest req)
        {
            var result = "";
            try
            {
                var oriReq = AesEncryptionHelper.DecryptToObject<SensitiveData>(req.EncryptedData, ConfigManager.SystemSection.AesKey, ConfigManager.SystemSection.AesIv);
                // query lol...
                var customerData = new CustomerDetailData()
                {
                    Account = oriReq.Account,
                    IdNumber = oriReq.IdNumber,
                    Phone = "09XXXXXXXX"
                };
                var oriResp = new ApiResponseBase<CustomerDetailData>()
                {
                    Data = customerData
                };
                result = AesEncryptionHelper.EncryptObject(customerData, ConfigManager.SystemSection.AesKey, ConfigManager.SystemSection.AesIv);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new BusinessException(ReturnCode.ExceptionError, ReturnCode.ExceptionError.GetDescription());
            }

            return result;
        }
    }
}


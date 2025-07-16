using System;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using Northwind.Models;
using Northwind.Utilities.CustExceptions;
using Northwind.Utilities.Enum;
using Northwind.Utilities.Extensions;
using Northwind.Utilities.Helper;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Northwind.Services.Test.implement
{
    public class TestService : BaseService, ITestService
    {
        private IGenericLogger _logger;
        public TestService(IGenericLogger logger)
        {
            _logger = logger;
        }

        public async Task<ApiResponseBase<bool>> GetConnectResult()
        {
            var result = new ApiResponseBase<bool>
            {
                Data = false
            };

            using (var context = base.NorthwindDB(ConnectionMode.Slave))
            {
                result.Data = context.Database.CanConnect();
            }

            return result;
        }

        public async Task<ApiResponseBase<bool>> RedisSetValue()
        {
            var result = new ApiResponseBase<bool>
            {
                Data = false
            };

            try
            {
                DateTime expirationTime = DateTime.UtcNow.AddMinutes(15);
                TimeSpan ttl = expirationTime - DateTime.UtcNow;

                //_logger.Info(ttl.ToString());

                await base.RedisService().SetStringAsync($"Login:aaaaaa", "ABcd1234", ttl);

                result.Data = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new BusinessException(ReturnCode.ExceptionError, ReturnCode.ExceptionError.GetDescription());
            }

            return result;
        }

        public async Task<ApiResponseBase<string>> RedisGetValue()
        {
            var result = new ApiResponseBase<string>
            {
                Data = ""
            };

            try
            {
                string redisKey = $"Login:aaaaaa";
                string? redisToken = await base.RedisService().GetStringAsync(redisKey);
                result.Data = redisToken;
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


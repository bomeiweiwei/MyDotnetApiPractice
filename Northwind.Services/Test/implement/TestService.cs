using System;
using Northwind.Models;
using Northwind.Utilities.Enum;

namespace Northwind.Services.Test.implement
{
	public class TestService:BaseService, ITestService
    {
		public TestService()
		{
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
    }
}


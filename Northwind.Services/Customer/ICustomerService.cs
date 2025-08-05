using System;
using Northwind.Models;
using Northwind.Models.External;

namespace Northwind.Services.Customer
{
	public interface ICustomerService
	{
        Task<string> GetCustomerDetailData(SecureRequest req);
    }
}


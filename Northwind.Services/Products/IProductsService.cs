using System;
using Northwind.Models;
using Northwind.Models.External;

namespace Northwind.Services.Products
{
	public interface IProductsService
	{
        // <summary>
        /// 取得產品資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApiResponseBase<GetProductResp>> GetProduct(int id);
    }
}


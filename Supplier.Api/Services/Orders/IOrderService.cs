using System;
using Supplier.Api.Models;
using Supplier.Api.Models.Api;

namespace Supplier.Api.Services.Orders
{
	public interface IOrderService
	{
		Task<ApiResponseBase<List<OrderData>>> GetOrderDatas(ApiPageRequestBase<QueryOrderArgs> req);
    }
}


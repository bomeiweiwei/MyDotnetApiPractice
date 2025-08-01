using System;
using Northwind.Models;
using Northwind.Models.External;

namespace Northwind.Repository.Supplier
{
	public interface ISupplierRepository
	{
        Task<ApiResponseBase<List<OrderData>>> GetOrderDatas(ApiPageRequestBase<QueryOrderArgs> req);

        Task<ApiResponseBase<ShippedData>> QueryShippedData(QueryShippedDataArgs req);
    }
}


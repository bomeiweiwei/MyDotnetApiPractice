using System;
using Supplier.Api.Models;
using Supplier.Api.Models.Api;
using Supplier.Api.Models.Config.External;

namespace Supplier.Api.Services.Orders
{
	public class OrderService: IOrderService
    {
        private List<OrderData> orders = new List<OrderData>()
        {
            new OrderData(){ OrderId=1,CustomerId=1,ShipName="AAA",ShipAddress="AAAAA",ShippedDate=DateTime.Now},
            new OrderData(){ OrderId=1,CustomerId=2,ShipName="BBB",ShipAddress="CCCCC"},
            new OrderData(){ OrderId=1,CustomerId=3,ShipName="CCC",ShipAddress="DDDDD",ShippedDate=DateTime.Now.AddMonths(-1)}
        };

        public async Task<ApiResponseBase<List<OrderData>>> GetOrderDatas(ApiPageRequestBase<QueryOrderArgs> req)
        {
            var result = new ApiResponseBase<List<OrderData>>()
            {
                Data = new List<OrderData>()
            };

            result.Data = orders.Where(m => m.ShippedDate.HasValue && m.ShippedDate.Value >= req.Data.ShippedStartDate && m.ShippedDate.Value <= req.Data.ShippedEndDate).ToList();
            result.Count = result.Data.Count();

            return result;
        }
    }
}


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

        private List<ShippedData> shippedDatas = new List<ShippedData>()
        {
            new ShippedData(){ Id=1,Contact="王大明",Address="AAAAAA"},
            new ShippedData(){ Id=2,Contact="王中明",Address="AAAAAA"},
            new ShippedData(){ Id=3,Contact="王小明",Address="AAAAAA"}
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

        public async Task<ApiResponseBase<ShippedData>> QueryShippedData(QueryShippedDataArgs req)
        {
            var result = new ApiResponseBase<ShippedData>()
            {
                Data = new ShippedData()
            };

            var query = shippedDatas.Where(m => m.Id == req.Id);
            if (!string.IsNullOrWhiteSpace(req.Contact))
            {
                query = query.Where(m => m.Contact.Contains(req.Contact));
            }

            result.Data = query.FirstOrDefault() ?? new ShippedData();

            return result;
        }
    }
}


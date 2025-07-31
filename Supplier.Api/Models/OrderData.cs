using System;
namespace Supplier.Api.Models
{
	public class OrderData
	{
		public int OrderId { get; set; }
		public int CustomerId { get; set; }
		public string ShipName { get; set; }
        public string ShipAddress { get; set; }
		public DateTime? ShippedDate { get; set; }
    }
}


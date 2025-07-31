using System;
namespace Northwind.Models.External
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


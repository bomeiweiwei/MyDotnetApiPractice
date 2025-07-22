using System;
namespace Northwind.Models.External
{
	public class GetProductResp
	{
        public int ProductID { get; set; }
        public string ProductName { get; set; }

        public int? SupplierId { get; set; }
        public string? Supplier { get; set; }

        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}


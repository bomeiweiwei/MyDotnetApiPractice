using System;
namespace Supplier.Api.Models
{
	public class QueryOrderArgs
	{
		public DateTime ShippedStartDate { get; set; }
        public DateTime ShippedEndDate { get; set; }
    }
}


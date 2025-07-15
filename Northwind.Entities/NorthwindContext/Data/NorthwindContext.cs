using System;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Entities.NorthwindContext.Data
{
	public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options) { }
    }
}


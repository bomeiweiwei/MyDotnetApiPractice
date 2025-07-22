using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;
using Northwind.Models.CustExceptions;
using Northwind.Models.External;
using Northwind.Services.Categories;
using Northwind.Utilities.Enum;
using Northwind.Utilities.Extensions;
using Northwind.Utilities.Helper;

namespace Northwind.Services.Products.implement
{
	public class ProductsService : BaseService, IProductsService
    {
        private IGenericLogger _logger;
        public ProductsService(IHttpContextAccessor httpContextAccessor, IGenericLogger logger) : base(httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task<ApiResponseBase<GetProductResp>> GetProduct(int id)
        {
            var result = new ApiResponseBase<GetProductResp>()
            {
                Data = new GetProductResp()
            };

            using (var context = base.NorthwindDB(ConnectionMode.Slave))
            {
                var query = await (from prod in context.Products.AsNoTracking()
                                   join cat in context.Categories.AsNoTracking() on prod.CategoryId equals cat.CategoryId into pc
                                   from subc in pc.DefaultIfEmpty()
                                   join sup in context.Suppliers.AsNoTracking() on prod.SupplierId equals sup.SupplierId into ps
                                   from subs in ps.DefaultIfEmpty()
                                   where prod.ProductId == id
                                   select new GetProductResp
                                   {
                                       ProductID = prod.ProductId,
                                       ProductName = prod.ProductName,
                                       CategoryId = subc != null ? subc.CategoryId : null,
                                       CategoryName = subc != null ? subc.CategoryName : null,
                                       SupplierId = subs != null ? subs.SupplierId : null,
                                       Supplier = subs != null ? subs.CompanyName : null,
                                   }).FirstOrDefaultAsync();
                if (query != null)
                {
                    result.Data = query;
                }
                else
                {
                    throw new BusinessException(ReturnCode.DataNotExisted, ReturnCode.DataNotExisted.GetDescription());
                }
            }

            return result;
        }
    }
}


using System;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;
using Northwind.Models.Categories;
using Northwind.Utilities.Enum;

namespace Northwind.Services.Categories.implement
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService()
        {
        }

        public async Task<ApiResponseBase<List<CategoryDetail>>> GetCategories()
        {
            var result = new ApiResponseBase<List<CategoryDetail>>
            {
                Data = new List<CategoryDetail>()
            };

            using (var context = base.NorthwindDB(ConnectionMode.Slave))
            {
                result.Data = await context.Categories.Select(m => new CategoryDetail()
                {
                    CategoryId = m.CategoryId,
                    CategoryName = m.CategoryName,
                    Description = m.Description
                }).ToListAsync();
            }

            return result;
        }

        public async Task<ApiResponseBase<int>> CreateCategory(CreateCategoryReq req)
        {
            var result = new ApiResponseBase<int>
            {
                Data = 0
            };

            return result;
        }
    }
}


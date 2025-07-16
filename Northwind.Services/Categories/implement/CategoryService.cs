using System;
using Microsoft.EntityFrameworkCore;
using Northwind.Entities.NorthwindContext.Models;
using Northwind.Models;
using Northwind.Models.Categories;
using Northwind.Utilities.CustExceptions;
using Northwind.Utilities.Enum;
using Northwind.Utilities.Extensions;
using Northwind.Utilities.Helper;

namespace Northwind.Services.Categories.implement
{
    public class CategoryService : BaseService, ICategoryService
    {
        private IGenericLogger _logger;
        public CategoryService(IGenericLogger logger)
        {
            _logger = logger;
        }

        public async Task<ApiResponseBase<List<CategoryDetail>>> GetCategories()
        {
            var result = new ApiResponseBase<List<CategoryDetail>>()
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
            var result = new ApiResponseBase<int>()
            {
                Data = 0
            };
            using (var context = base.NorthwindDB(ConnectionMode.Master))
            {
                var query = await context.Categories.FirstOrDefaultAsync(m => m.CategoryName == req.CategoryName);
                if (query != null)
                {
                    throw new BusinessException(ReturnCode.DataAlreadyExisted, ReturnCode.DataAlreadyExisted.GetDescription());
                }

                var executionStrategy = context.Database.CreateExecutionStrategy();
                await executionStrategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync();
                    try
                    {
                        Category category = new Category()
                        {
                            CategoryName = req.CategoryName,
                            Description = req.Description,
                        };

                        await context.Categories.AddAsync(category);
                        await context.SaveChangesAsync();

                        result.Data = category.CategoryId;

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.Error(ex.Message);
                        throw new BusinessException(ReturnCode.ExceptionError, ReturnCode.ExceptionError.GetDescription());
                    }
                });
            }
            return result;
        }
    }
}


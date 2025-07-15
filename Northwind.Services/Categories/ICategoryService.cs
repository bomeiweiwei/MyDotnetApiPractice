using System;
using Northwind.Models;
using Northwind.Models.Categories;

namespace Northwind.Services.Categories
{
    public interface ICategoryService
	{
        Task<ApiResponseBase<List<CategoryDetail>>> GetCategories();
        /// <summary>
        /// 新增類別
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ApiResponseBase<int>> CreateCategory(CreateCategoryReq req);
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Northwind.Models.Categories;
using Northwind.Services.Categories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route("GetCategories")]
        public async Task<ApiResponseBase<List<CategoryDetail>>> GetCategories()
        {
            return await _categoryService.GetCategories();
        }
        [HttpPost]
        [Route("CreateCategory")]
        public async Task<ApiResponseBase<int>> CreateCategory(CreateCategoryReq req)
        {
            return await _categoryService.CreateCategory(req);
        }
    }
}


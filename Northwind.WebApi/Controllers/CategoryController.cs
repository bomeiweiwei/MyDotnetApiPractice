﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Northwind.Models.Categories;
using Northwind.Services.Categories;
using Northwind.Utilities.Enum;
using Northwind.WebApi.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtAuthActionFilter]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        [Route("GetCategories")]
        [PermissionAuthorize(PermissionCode.ManageCategories, PermissionCode.ViewCategories)]
        public async Task<IActionResult> GetCategories()
        {
            var data = await _categoryService.GetCategories();
            return Ok(data);
        }
        [HttpPost]
        [Route("CreateCategory")]
        [PermissionAuthorize(PermissionCode.ManageCategories, PermissionCode.CreateCategory)]
        public async Task<IActionResult> CreateCategory(CreateCategoryReq req)
        {
            var data = await _categoryService.CreateCategory(req);
            return Ok(data);
        }
    }
}


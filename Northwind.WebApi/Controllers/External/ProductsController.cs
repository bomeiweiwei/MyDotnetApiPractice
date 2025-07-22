using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Northwind.Models.External;
using Northwind.Services.Products;
using Northwind.WebApi.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.WebApi.Controllers.External
{
    [ApiController]
    [Route("api/External/[controller]")]
    [ApiKeyAuthFilter]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }
        [HttpGet]
        [Route("GetProduct")]
        public async Task<ApiResponseBase<GetProductResp>> GetProduct(int id)
        {
            return await _productsService.GetProduct(id);
        }
    }
}


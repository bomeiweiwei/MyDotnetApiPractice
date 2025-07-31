using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Northwind.Models.External;
using Northwind.Repository.Supplier;
using Northwind.WebApi.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtAuthActionFilter]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;
        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        [HttpPost]
        [Route("GetOrderDatas")]
        public async Task<IActionResult> GetOrderDatas(ApiPageRequestBase<QueryOrderArgs> req)
        {
            var resp = await _supplierRepository.GetOrderDatas(req);
            return Ok(resp);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Models;
using Northwind.Models.External;
using Northwind.Services.Customer;
using Northwind.WebApi.Filters;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.WebApi.Controllers.External
{
    [ApiController]
    [Route("api/External/[controller]")]
    [ApiKeyAuthFilter]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [Route("GetCustomerDetailData")]
        public async Task<IActionResult> GetCustomerDetailData(SecureRequest req)
        {
            var resp = await _customerService.GetCustomerDetailData(req);
            return Ok(resp);
        }
    }
}


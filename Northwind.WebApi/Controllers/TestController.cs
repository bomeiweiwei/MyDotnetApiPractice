using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Entities.NorthwindContext.Data;
using Northwind.Services.Test;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet("check")]
        public async Task<IActionResult> Check()
        {
            var canConnect = await _testService.GetConnectResult();
            return Ok(new { dbConnected = canConnect.Data });
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Entities.NorthwindContext.Data;
using Northwind.Models;
using Northwind.Services.Test;
using Northwind.Utilities.ConfigManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ITestService _testService;

        public TestController(IWebHostEnvironment env, ITestService testService)
        {
            _env = env;
            _testService = testService;
        }
        [HttpGet]
        [Route("env")]
        public IActionResult GetEnv()
        {
            return Ok(_env.EnvironmentName);
        }
        [HttpGet]
        [Route("check")]
        public async Task<IActionResult> Check()
        {
            var canConnect = await _testService.GetConnectResult();
            return Ok(new { dbConnected = canConnect.Data });
        }

        [HttpGet]
        [Route("RedisSetValue")]
        public async Task<IActionResult> RedisSetValue()
        {
            return Ok(await _testService.RedisSetValue());
        }

        [HttpGet]
        [Route("RedisGetValue")]
        public async Task<IActionResult> RedisGetValue()
        {
            return Ok(await _testService.RedisGetValue());
        }

        [HttpGet("GetSystemInfo")]
        public IActionResult GetSystemInfo()
        {
            return Ok(new
            {
                apiKey = ConfigManager.SystemSection.ApiKey,
                headerName = ConfigManager.SystemSection.HeaderName,
                allowedOrigins = ConfigManager.SystemSection.WithOrigins
            });
        }

        [HttpGet]
        [Route("GetFakeSupplierData")]
        public IActionResult GetFakeSupplierData()
        {
            string apiKey = ConfigManager.ExternalSystemsSection.FakeSupplier.ApiKey;
            string header = ConfigManager.ExternalSystemsSection.FakeSupplier.HeaderName;
            return Ok(new { apiKey, header });
        }

        [HttpGet]
        [Route("GetFakeBank1Data")]
        public IActionResult GetFakeBank1Data()
        {
            var test = ConfigManager.ExternalSystemsSection.GetSystem("FakeBank1");

            string apiKey = test.ApiKey;
            string header = test.HeaderName;
            return Ok(new { apiKey, header });
        }
    }
}


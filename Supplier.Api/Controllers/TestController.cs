using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Supplier.Api.Models;
using Supplier.Api.Models.Api;
using Supplier.Api.Models.Config.External;
using Supplier.Api.Models.Config.Sys;
using Supplier.Api.Services.Test;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Supplier.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly SystemSettings _systemSettings;
        private readonly ITestService _testService;
        private readonly ExternalSystemsOptions _systems;

        public TestController(IWebHostEnvironment env, ITestService testService, IOptions<SystemSettings> systemOptions, IOptions<ExternalSystemsOptions> options)
        {
            _env = env;
            _testService = testService;
            _systemSettings = systemOptions.Value;
            _systems = options.Value;
        }

        [HttpGet("GetSystemInfo")]
        public IActionResult GetSystemInfo()
        {
            return Ok(new
            {
                apiKey = _systemSettings.ApiKey,
                headerName = _systemSettings.HeaderName,
                allowedOrigins = _systemSettings.WithOrigins
            });
        }

        [HttpGet]
        [Route("OptionsGetValue")]
        public async Task<ApiResponseBase<OptionsGetValueResp>> OptionsGetValue()
        {
            return await _testService.OptionsGetValue();
        }

        [HttpGet]
        [Route("GetNorthwindData")]
        public IActionResult GetNorthwindData()
        {
            string apiKey = "";
            string header = "";
            if (_systems.TryGetValue("Northwind", out var config))
            {
                apiKey = config.ApiKey;
                header = config.HeaderName;
            }
            return Ok(new { apiKey, header });
        }
    }
}


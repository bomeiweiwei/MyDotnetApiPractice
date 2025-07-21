using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Supplier.Api.Models;
using Supplier.Api.Models.Api;
using Supplier.Api.Models.Identity;
using Supplier.Api.Services.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Supplier.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ILoginService _service;
        public IdentityController(ILoginService service)
        {
            _service = service;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginReq req)
        {
            var resp = await _service.Login(req);
            return Ok(resp);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Entities.NorthwindContext.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly NorthwindContext _context;

        public TestController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet("check")]
        public IActionResult Check()
        {
            var canConnect = _context.Database.CanConnect();
            return Ok(new { dbConnected = canConnect });
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Supplier.Api.Filters;
using Supplier.Api.Models;
using Supplier.Api.Models.Api;
using Supplier.Api.Models.Config.Sys;
using Supplier.Api.Services.Orders;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Supplier.Api.Controllers
{
    [ApiKeyAuth]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("GetOrderDatas")]
        public async Task<IActionResult> GetOrderDatas(ApiPageRequestBase<QueryOrderArgs> req)
        {
            var resp = await _orderService.GetOrderDatas(req);
            return Ok(resp);
        }
    }
}


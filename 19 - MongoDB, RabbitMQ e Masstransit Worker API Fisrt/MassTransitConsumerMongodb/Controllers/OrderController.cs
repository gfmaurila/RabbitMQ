using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using masstransit_consumer_api_first.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace masstransit_consumer_api_first.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IBusControl _bus;

        public OrderController(ILogger<OrderController> logger, IBusControl bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            await _bus.Publish(order);

            _logger.LogInformation($"Message received. OrderId: {order.OrderId}");

            return Ok($"{DateTime.Now:o}");
        }

        [HttpGet]
        public async Task<IActionResult> Get(long orderid, string productname)
        {
            Order order = new Order() 
            { 
                OrderId = orderid,
                ProductName = productname
            };

            await _bus.Publish(order);

            _logger.LogInformation($"Message received. OrderId: {order.OrderId}");

            return Ok($"{DateTime.Now:o}");
        }
    }
}


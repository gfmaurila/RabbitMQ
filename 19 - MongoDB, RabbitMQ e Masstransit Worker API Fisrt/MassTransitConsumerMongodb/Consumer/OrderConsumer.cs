using MassTransit;
using masstransit_consumer_api_first.Domain;
using masstransit_consumer_api_first.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace masstransit_consumer_api_first.Consumer
{
    public sealed class OrderConsumer : IConsumer<Order>
    {
        private readonly ILogger<OrderConsumer> _logger;
        private readonly IOrderRepository _orderRepository;


        public OrderConsumer(ILogger<OrderConsumer> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public Task Consume(ConsumeContext<Order> context)
        {
            try
            {                   
                var order = context.Message;

                _logger.LogInformation($"Message received on consumer: {order.OrderId}");

                _orderRepository.Insert(order);

                _logger.LogInformation($"Order received: {order.OrderId}");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error on try to consume order", ex);
            }

            return Task.CompletedTask;
        }
    }
}

using masstransit_consumer_api_first.Domain;
using masstransit_consumer_api_first.Repository.Factory;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace masstransit_consumer_api_first.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(ILogger<OrderRepository> logger)
        {
            _logger = logger;
        }

        public void Insert(Order order)
        {
            try
            {
                var collection = MongoDBConnectionFactory<Order>.GetCollection();

                collection.InsertOne(order);
            }catch(Exception ex)
            {
                _logger.LogError($"Erro ao tentar inserir um novo pedido. OrderId: {order.OrderId}", ex);
                throw;
            }
        }
    }
}

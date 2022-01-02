using masstransit_consumer_api_first.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace masstransit_consumer_api_first.Repository
{
    public interface IOrderRepository
    {
        void Insert(Order order);
    }
}

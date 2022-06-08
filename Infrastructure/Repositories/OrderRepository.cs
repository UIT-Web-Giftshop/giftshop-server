using Domain.Entities.Order;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(IMongoContext context) : base(context)
        {

        }
    }
}
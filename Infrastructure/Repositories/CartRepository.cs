using Domain.Entities.Cart;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }
    }
}
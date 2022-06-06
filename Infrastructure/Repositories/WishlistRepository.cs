using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class WishlistRepository : RefactorRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }
    }
}
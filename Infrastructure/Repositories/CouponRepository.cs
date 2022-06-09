using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        public CouponRepository(IMongoContext mongoContext) : base(mongoContext)
        {
        }
    }
}
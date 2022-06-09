using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Coupons.Queries
{
    public class GetCouponQueryHandler : IRequestHandler<GetCouponQuery, ResponseApi<Coupon>>
    {
        private readonly ICouponRepository _couponRepository;

        public GetCouponQueryHandler(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<ResponseApi<Coupon>> Handle(GetCouponQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _couponRepository.GetOneAsync(request.Id, cancellationToken);
            return ResponseApi<Coupon>.ResponseOk(coupon);
        }
    }
}
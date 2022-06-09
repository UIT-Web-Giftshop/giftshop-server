using Application.Commons;
using Domain.Entities;
using MediatR;

namespace Application.Features.Coupons.Queries
{
    public class GetCouponQuery : IRequest<ResponseApi<Coupon>>
    {
        public string Id { get; set; }
    }
}
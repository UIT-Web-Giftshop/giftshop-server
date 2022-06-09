using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Coupons.Commands.Delete
{
    public class DeleteCouponsCommandHandler : IRequestHandler<DeleteCouponsCommand, ResponseApi<Unit>>
    {
        private readonly ICouponRepository _couponRepository;

        public DeleteCouponsCommandHandler(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(DeleteCouponsCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<Coupon, bool>> filter = q => request.Ids.Contains(q.Id);

            var deleted = await _couponRepository.DeleteManyAsync(
                filter, cancellationToken);

            return ResponseApi<Unit>.ResponseOk(Unit.Value);
        }
    }
}
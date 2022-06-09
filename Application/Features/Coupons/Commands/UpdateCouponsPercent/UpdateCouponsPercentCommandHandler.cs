using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Driver;

namespace Application.Features.Coupons.Commands.UpdateCouponsPercent
{
    public class UpdateCouponsPercentCommandHandler : IRequestHandler<UpdateCouponsPercentCommand, ResponseApi<Unit>>
    {
        private readonly ICouponRepository _couponRepository;

        public UpdateCouponsPercentCommandHandler(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(UpdateCouponsPercentCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _couponRepository.UpdateManyAsync(
                    q => request.CouponCodes.Contains(q.Id),
                    q => q.Set(o => o.DiscountPercent, request.NewPercent),
                    new UpdateOptions() { IsUpsert = false },
                    cancellationToken);

                return ResponseApi<Unit>.ResponseOk(Unit.Value);
            }
            catch (Exception)
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_EXECUTING);
            }
        }
    }
}
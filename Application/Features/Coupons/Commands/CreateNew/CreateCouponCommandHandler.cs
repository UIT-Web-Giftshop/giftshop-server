using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Infrastructure.Interfaces.Services;
using MediatR;

namespace Application.Features.Coupons.Commands.CreateNew
{
    public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, ResponseApi<Unit>>
    {
        private readonly IDiscountService _discountService;

        public CreateCouponCommandHandler(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        public Task<ResponseApi<Unit>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            if (request.ValidFrom > request.ValidTo)
                return Task.FromResult(ResponseApi<Unit>.ResponseFail("Ngày không hợp lệ"));
            
            var task = _discountService.GenerateCoupon(request.Percent, request.EventCode, request.ValidFrom,
                request.ValidTo, request.Number);
            task.Wait(cancellationToken);

            return Task.FromResult(ResponseApi<Unit>.ResponseOk(Unit.Value));
        }
    }
}
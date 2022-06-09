using System.Collections.Generic;
using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Coupons.Commands.UpdateCouponsPercent
{
    public class UpdateCouponsPercentCommand : IRequest<ResponseApi<Unit>>
    {
        public List<string> CouponCodes { get; set; }
        public float NewPercent { get; set; }
    }

    public sealed class UpdateCouponsPercentQueryValidator : AbstractValidator<UpdateCouponsPercentCommand>
    {
        public UpdateCouponsPercentQueryValidator()
        {
            RuleFor(x => x.NewPercent)
                .GreaterThan(0)
                .LessThan(100);
        }
    }
}
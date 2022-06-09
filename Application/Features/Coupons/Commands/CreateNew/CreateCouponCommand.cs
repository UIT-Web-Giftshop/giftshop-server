using System;
using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Coupons.Commands.CreateNew
{
    public class CreateCouponCommand : IRequest<ResponseApi<Unit>>
    {
         public int Number { get; set; }
         public float Percent { get; set; }
        public string EventCode { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }

    public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
    {
        public CreateCouponCommandValidator()
        {
            RuleFor(x => x.Number)
                .GreaterThan(0);
            RuleFor(x => x.Percent)
                .GreaterThan(0).LessThan(100);
            RuleFor(x => x.ValidTo)
                .GreaterThan(DateTime.UtcNow);
        }
    }
}
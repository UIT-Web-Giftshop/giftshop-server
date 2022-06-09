#nullable enable
using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class CreateProfileOrderCommand : IRequest<ResponseApi<Unit>>
    {
        public string? CouponCode { get; set; }
        
        public string Address { get; set; }
    }

    public class CreateProfileOrderCommandValidator : AbstractValidator<CreateProfileOrderCommand>
    {
        public CreateProfileOrderCommandValidator()
        {
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ không được để trống");
        }
    }
}
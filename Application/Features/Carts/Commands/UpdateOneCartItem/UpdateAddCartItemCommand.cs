using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Carts.Commands.UpdateOneCartItem
{
    public class UpdateAddCartItemCommand : IRequest<ResponseApi<Unit>>
    {
        public string Sku { get; init; }
        public int Quantity { get; init; }
    }

    public class UpdateAddCartItemCommandValidator : AbstractValidator<UpdateAddCartItemCommand>
    {
        public UpdateAddCartItemCommandValidator()
        {
            RuleFor(x => x.Sku).NotEmpty().WithMessage("Mã sản phẩm không hợp lệ");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0");
        }
    }
}
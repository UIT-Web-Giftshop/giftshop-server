using FluentValidation;

namespace Application.Features.Carts.Commands.UpdateOneCartItem
{
    public class UpdateDeleteCartItemCommand : UpdateAddCartItemCommand
    {
        
    }

    public class UpdateDeleteCartItemCommandValidator : AbstractValidator<UpdateAddCartItemCommand>
    {
        public UpdateDeleteCartItemCommandValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Số lượng phải lớn hơn 0");
            RuleFor(x => x.Sku).NotEmpty().WithMessage("Mã sản phẩm không hợp lệ");
        }
    }
}
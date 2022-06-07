using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Products.Commands.UpdateOneProductStock
{
    public class UpdateOneProductStockCommand : IRequest<ResponseApi<Unit>>
    {
        public string Sku { get; init; }
        public int Stock { get; init; }
    }

    public sealed class UpdateOneProductStockCommandValidator : AbstractValidator<UpdateOneProductStockCommand>
    {
        public UpdateOneProductStockCommandValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("Mã sản phẩm không được để trống");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Giá sản phẩm phải lớn hơn 0");
        }
    }
}
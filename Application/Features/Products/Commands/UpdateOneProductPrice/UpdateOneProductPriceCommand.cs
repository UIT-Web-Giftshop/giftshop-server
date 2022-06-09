using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Products.Commands.UpdateOneProductPrice
{
    public class UpdateOneProductPriceCommand : IRequest<ResponseApi<Unit>>
    { 
        public string Sku { get; init; }
        public double Price { get; set; }
    }

    public sealed class UpdateOneProductPriceCommandValidator : AbstractValidator<UpdateOneProductPriceCommand>
    {
        public UpdateOneProductPriceCommandValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("Mã sản phẩm không được để trống");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0");
        }
    }
}
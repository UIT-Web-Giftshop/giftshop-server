using Application.Commons;
using Domain.ViewModels.Product;
using FluentValidation;
using MediatR;

namespace Application.Features.Products.Commands
{
    public class AddOneProductCommand : IRequest<ResponseApi<string>>
    {
        public ProductDetailViewModel Product { get; init; }
    }

    public sealed class AddOneProductCommandValidator : AbstractValidator<ProductDetailViewModel>
    {
        public AddOneProductCommandValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("Sku không được trống");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên sản phẩm không được trống");
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Giá sản phẩm không được trống")
                .GreaterThan(0).WithMessage("Giá sản phẩm phải lớn hơn 0");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng sản phẩm phải lớn hơn hoặc bằng 0");
        }
    }
}
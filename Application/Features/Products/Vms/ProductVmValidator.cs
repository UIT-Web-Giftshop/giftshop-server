using Application.Features.Objects.Vms;
using FluentValidation;

namespace Application.Features.Products.Vms
{
    public sealed class ProductVmValidator : AbstractValidator<ProductVm>, IObjectValidator
    {
        public ProductVmValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }
}
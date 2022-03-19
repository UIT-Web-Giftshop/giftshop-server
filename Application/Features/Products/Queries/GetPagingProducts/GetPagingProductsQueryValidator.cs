using Application.Features.Objects.Queries.GetPagingObjects;
using FluentValidation;

namespace Application.Features.Products.Queries.GetPagingProducts
{
    public sealed class GetPagingProductsQueryValidator : AbstractValidator<GetPagingProductsQuery>,
        IQueryValidator
    {
        public GetPagingProductsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThan(0).LessThan(10000);
            RuleFor(x => x.PageSize).GreaterThan(0).LessThan(100);
        }
    }
}
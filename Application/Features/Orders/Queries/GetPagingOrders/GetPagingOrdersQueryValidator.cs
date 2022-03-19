using Application.Features.Objects.Queries.GetPagingObjects;
using FluentValidation;

namespace Application.Features.Orders.Queries.GetPagingOrders
{
    public sealed class GetPagingOrdersQueryValidator : AbstractValidator<GetPagingOrdersQuery>, 
        IQueryValidator
    {
        public GetPagingOrdersQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThan(0).LessThan(10000);
            RuleFor(x => x.PageSize).GreaterThan(0).LessThan(100);
        }
    }
}
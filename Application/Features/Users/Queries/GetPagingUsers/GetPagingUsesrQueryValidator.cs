using Application.Features.Objects.Queries.GetPagingObjects;
using FluentValidation;

namespace Application.Features.Users.Queries.GetPagingUsers
{
    public sealed class GetPagingUsesrQueryValidator : AbstractValidator<GetPagingUsersQuery>,
        IQueryValidator
    {
        public GetPagingUsesrQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThan(0).LessThan(10000);
            RuleFor(x => x.PageSize).GreaterThan(0).LessThan(100);
        }
    }
}
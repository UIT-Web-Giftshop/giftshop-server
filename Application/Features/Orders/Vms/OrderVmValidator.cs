using System;
using Application.Features.Objects.Vms;
using FluentValidation;

namespace Application.Features.Orders.Vms
{
    public sealed class OrderVmValidator : AbstractValidator<OrderVm>, IObjectValidator
    {
        public OrderVmValidator()
        {
            RuleFor(x => x.Items).NotNull();
            RuleFor(x => x.IsPaid).NotNull();
            RuleFor(x => x.Status).NotNull();
            RuleFor(x => x.TotalPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CreatedAt).GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(x => x.SuccessAt).GreaterThanOrEqualTo(DateTime.Now);
        }
    }
}
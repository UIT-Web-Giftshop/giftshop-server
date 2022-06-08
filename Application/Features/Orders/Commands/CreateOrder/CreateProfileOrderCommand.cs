using System.Collections.Generic;
using Application.Commons;
using FluentValidation;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class CreateProfileOrderCommand : IRequest<ResponseApi<Unit>>
    {
        public Dictionary<string, int> OrderItems { get; set; }
    }
    
    public class CreateProfileOrderCommandValidator : AbstractValidator<CreateProfileOrderCommand>
    {
        public CreateProfileOrderCommandValidator()
        {
            RuleForEach(x => x.OrderItems.Values).GreaterThan(0).WithMessage("Số lượng sản phẩm phải lớn hơn 0");
        }
    }
}
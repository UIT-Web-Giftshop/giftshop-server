#nullable enable
using Application.Commons;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class CreateProfileOrderCommand : IRequest<ResponseApi<Unit>>
    {
        public string? CouponCode { get; set; }
    }
}
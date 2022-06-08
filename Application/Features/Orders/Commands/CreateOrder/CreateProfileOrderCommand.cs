using System.Collections.Generic;
using Application.Commons;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public class CreateProfileOrderCommand : IRequest<ResponseApi<Unit>>
    {
        public Dictionary<string, int> OrderItems { get; set; }
    }
}
using Application.Commons;
using Domain.Entities.Order;
using MediatR;

namespace Application.Features.Orders.Queries.GetOneOrderById
{
    public class GetOneProfileOrderQuery : IRequest<ResponseApi<Order>>
    {
        public string Id { get; set; }
    }
}
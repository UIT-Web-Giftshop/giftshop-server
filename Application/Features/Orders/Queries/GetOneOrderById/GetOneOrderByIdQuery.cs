using Application.Commons;
using Application.Features.Objects.Queries.GetOneObject;
using Application.Features.Orders.Vms;
using MediatR;

namespace Application.Features.Orders.Queries.GetOneOrderById
{
    public class GetOneOrderByIdQuery : GetOneObjectQuery, IRequest<ResponseApi<OrderVm>>
    {
        public string Id { get; set; }
    }
}
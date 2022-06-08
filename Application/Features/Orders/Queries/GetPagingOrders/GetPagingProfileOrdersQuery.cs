using Application.Commons;
using Domain.Entities.Order;
using Domain.Paging;
using MediatR;

namespace Application.Features.Orders.Queries.GetPagingOrders
{
    public class GetPagingProfileOrdersQuery : IRequest<ResponseApi<PagingModel<Order>>>
    {
        public PagingRequest PagingRequest { get; set; }
        public string SortBy { get; set; }
        public bool IsDesc { get; set; }
    }
}
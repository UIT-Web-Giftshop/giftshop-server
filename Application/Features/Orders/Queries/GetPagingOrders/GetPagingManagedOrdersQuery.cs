#nullable enable
using Application.Commons;
using Domain.Entities.Order;
using Domain.Paging;
using MediatR;

namespace Application.Features.Orders.Queries.GetPagingOrders
{
    public class GetPagingManagedOrdersQuery : IRequest<ResponseApi<PagingModel<Order>>>
    {
        public PagingRequest PagingRequest { get; set; }
        public string? Status { get; set; }
        public string FilterUser { get; set; }
        public string SortBy { get; set; }
        public bool IsDesc { get; set; }
    }
}
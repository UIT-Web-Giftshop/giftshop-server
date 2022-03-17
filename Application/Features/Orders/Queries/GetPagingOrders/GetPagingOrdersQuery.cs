using Application.Commons;
using Application.Features.Objects.Queries.GetPagingObjects;
using Application.Features.Orders.Vms;
using Domain.Paging;
using MediatR;

namespace Application.Features.Orders.Queries.GetPagingOrders
{
    public class GetPagingOrdersQuery : GetPagingObjectsQuery, IRequest<ResponseApi<PagingModel<OrderVm>>>
    {

    }
}
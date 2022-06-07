using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Application.Features.Objects.Queries.GetPagingObjects;
using Application.Features.Orders.Vms;
using AutoMapper;
using Domain.Entities.Order;
using Domain.Paging;
using Infrastructure.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Orders.Queries.GetPagingOrders
{
    public class GetPagingOrdersHandler : GetPagingObjectsHandler<Order, OrderVm>,
        IRequestHandler<GetPagingOrdersQuery, ResponseApi<PagingModel<OrderVm>>>
    {
        public GetPagingOrdersHandler(IOrderRepository _orderRepository, IMapper _mapper, 
            ICounterRepository counterRepository) : base(_orderRepository, _mapper, 
            counterRepository)
        {

        }

        public override Expression<Func<Order, bool>> GetExpression(GetPagingObjectsQuery request)
        {
            return string.IsNullOrWhiteSpace(request.Search) ? null : p =>
                p.UserId.Contains(request.Search);
        }

        public override Expression<Func<Order, object>> GetSortExpression(GetPagingObjectsQuery 
            request)
        {
            return q => q.TotalPrice;
        }

        public async Task<ResponseApi<PagingModel<OrderVm>>> Handle(GetPagingOrdersQuery request,
            CancellationToken cancellationToken)
        {
            return await base.Handle(request, cancellationToken);
        }
    }
}
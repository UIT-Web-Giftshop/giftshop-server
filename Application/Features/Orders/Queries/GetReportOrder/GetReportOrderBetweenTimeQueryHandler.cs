using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons; 
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;
using MediatR;
using MongoDB.Driver;

namespace Application.Features.Orders.Queries.GetReportOrder
{
    public class GetReportOrderBetweenTimeQueryHandler : IRequestHandler<GetReportOrderBetweenTimeQuery, ResponseApi<List<Order>>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetReportOrderBetweenTimeQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ResponseApi<List<Order>>> Handle(GetReportOrderBetweenTimeQuery request, CancellationToken cancellationToken)
        {
            var filter = PrepareFilter(request);
            var data = await _orderRepository.FindAsync(filter, null, cancellationToken);
            return ResponseApi<List<Order>>.ResponseOk(data.ToList(cancellationToken));
        }

        private static Expression<Func<Order, bool>> PrepareFilter(GetReportOrderBetweenTimeQuery request)
        {
            Expression<Func<Order, bool>> filter = q => request.From < q.CreatedAt && q.CreatedAt < request.To;
            if (!request.AllStatus)
            {
                filter = q => q.Status.Equals(request.Status) && request.From < q.CreatedAt && q.CreatedAt < request.To;
            }
            return filter;
        }
    }
}
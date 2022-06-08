using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;

namespace Application.Features.Orders.Queries.GetOneOrderById
{
    public class GetOneProfileOrderQueryHandler : IRequestHandler<GetOneProfileOrderQuery, ResponseApi<Order>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;

        public GetOneProfileOrderQueryHandler(IOrderRepository orderRepository, IAccessorService accessorService)
        {
            _orderRepository = orderRepository;
            _accessorService = accessorService;
        }

        public async Task<ResponseApi<Order>> Handle(GetOneProfileOrderQuery request, CancellationToken cancellationToken)
        {
            var email = _accessorService.Email();
            var order = await _orderRepository.FindOneAsync(x => x.Id == request.Id && x.UserEmail == email, cancellationToken);
            
            if (order is null)
                return ResponseApi<Order>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);
            
            return ResponseApi<Order>.ResponseOk(order);
        }
    }
}
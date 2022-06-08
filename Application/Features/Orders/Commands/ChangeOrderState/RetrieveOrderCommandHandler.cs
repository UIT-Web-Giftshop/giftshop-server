using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities.Order;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Orders.Commands.ChangeOrderState
{
    public class RetrieveOrderCommandHandler : IRequestHandler<RetrieveOrderCommand, ResponseApi<Unit>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;

        public RetrieveOrderCommandHandler(IAccessorService accessorService, IOrderRepository orderRepository)
        {
            _accessorService = accessorService;
            _orderRepository = orderRepository;
        }

        public async Task<ResponseApi<Unit>> Handle(RetrieveOrderCommand request, CancellationToken cancellationToken)
        {
            var email = _accessorService.Email();
            var order = await _orderRepository.GetOneAsync(request.Id, cancellationToken);
            if (order == null)
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);

            if (order.UserEmail != email)
                return ResponseApi<Unit>.ResponseFail(StatusCodes.Status403Forbidden);

            if (order.Status is nameof(OrderStatus.Success))
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_UPDATE_STATUS_NOT_ALLOWED);

            var updated = await _orderRepository.UpdateOneAsync(
                order.Id,
                x => x.Set(o => o.Status, nameof(OrderStatus.Pending)),
                cancellationToken: cancellationToken);

            if (updated.AnyDocumentModified())
            {
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Đặt lại đơn hàng thành công");
            }

            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError,
                ResponseConstants.ERROR_EXECUTING);
        }
    }
}
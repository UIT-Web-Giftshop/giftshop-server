using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities.Order;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using RazorEmailLibs.Constants;
using RazorEmailLibs.Views.Emails;

namespace Application.Features.Orders.Commands.ChangeOrderState
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, ResponseApi<Unit>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;
        private readonly IMailService _mailService;


        public CancelOrderCommandHandler(IOrderRepository orderRepository, IAccessorService accessorService, IMailService mailService)
        {
            _orderRepository = orderRepository;
            _accessorService = accessorService;
            _mailService = mailService;
        }

        public async Task<ResponseApi<Unit>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var email = _accessorService.Email();
            var order = await _orderRepository.GetOneAsync(request.Id, cancellationToken);
            
            if (order == null)
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);

            if (order.UserEmail != email)
                return ResponseApi<Unit>.ResponseFail(StatusCodes.Status403Forbidden);

            if (order.Status is nameof(OrderStatus.Canceled) or nameof(OrderStatus.Success))
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_UPDATE_STATUS_NOT_ALLOWED);

            var mailModel = new CanceledOrderEmailViewModel(order,
                "Bạn đã hủy đơn hàng này");
            
            await _mailService.SendWithTemplate(order.UserEmail, "Đơn hàng đã hủy",
                new List<IFormFile>(), MailTemplatesName.CANCEL_ORDER_EMAIL, mailModel);
            
            var updated = await _orderRepository.UpdateOneAsync(
                order.Id,
                x => x.Set(o => o.Status, nameof(OrderStatus.Canceled)),
                cancellationToken: cancellationToken);

            if (updated.AnyDocumentModified())
            {
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Hủy đơn hàng thành công");
            }

            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError,
                ResponseConstants.ERROR_EXECUTING);
        }
    }
}
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
    public class RetrieveOrderCommandHandler : IRequestHandler<RetrieveOrderCommand, ResponseApi<Unit>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;
        private readonly IMailService _mailService;

        public RetrieveOrderCommandHandler(IAccessorService accessorService, IOrderRepository orderRepository, IMailService mailService)
        {
            _accessorService = accessorService;
            _orderRepository = orderRepository;
            _mailService = mailService;
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
                var mailModel = new ReceiptEmailViewModel(order);
                await _mailService.SendWithTemplate(order.UserEmail, "Đặt lại đơn hàng", new List<IFormFile>(), MailTemplatesName.RECEIPT_EMAIL, mailModel);
                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Đặt lại đơn hàng thành công");
            }

            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError,
                ResponseConstants.ERROR_EXECUTING);
        }
    }
}
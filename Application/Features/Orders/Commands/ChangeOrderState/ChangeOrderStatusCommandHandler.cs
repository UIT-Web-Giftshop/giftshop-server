using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Commons;
using Domain.Entities.Account;
using Domain.Entities.Order;
using Infrastructure.Extensions.Mongo;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using RazorEmailLibs.Constants;
using RazorEmailLibs.Views.Emails;

namespace Application.Features.Orders.Commands.ChangeOrderState
{
    public class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommand, ResponseApi<Unit>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;
        private readonly IMailService _mailService;


        public ChangeOrderStatusCommandHandler(IOrderRepository orderRepository, IMailService mailService,
            IAccessorService accessorService)
        {
            _orderRepository = orderRepository;
            _mailService = mailService;
            _accessorService = accessorService;
        }

        public async Task<ResponseApi<Unit>> Handle(ChangeOrderStatusCommand request,
            CancellationToken cancellationToken)
        {
            var email = _accessorService.Email();
            var role = _accessorService.Role();
            var allow = false;

            var order = await _orderRepository.GetOneAsync(request.Id, cancellationToken);

            if (order is null)
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_NOT_FOUND_ITEM);

            if (role == nameof(UserRoles.ADMIN)) allow = true;

            if (!allow)
            {
                return ResponseApi<Unit>.ResponseFail(StatusCodes.Status403Forbidden);
            }


            if (!CheckConstraintStatusUpdate(request, order))
            {
                return ResponseApi<Unit>.ResponseFail(ResponseConstants.ERROR_UPDATE_STATUS_NOT_ALLOWED);
            }

            var update = PrepareUpdateDefinition(request);

            var updated = await _orderRepository.UpdateOneAsync(
                request.Id,
                update,
                cancellationToken: cancellationToken
            );

            if (updated.AnyDocumentModified())
            {
                // send mail as admin
                if (request.Status == nameof(OrderStatus.Canceled))
                {
                    var mailModel = new CanceledOrderEmailViewModel(order,
                        "Cửa hàng đã hủy đơn của bạn vì tồn kho không đáp ứng đủ");
                    await _mailService.SendWithTemplate(order.UserEmail, "Đơn hàng đã hủy",
                        new List<IFormFile>(), MailTemplatesName.CANCEL_ORDER_EMAIL, mailModel);
                }

                return ResponseApi<Unit>.ResponseOk(Unit.Value, "Cập nhập trạng thái thành công");
            }

            return ResponseApi<Unit>.ResponseFail(StatusCodes.Status500InternalServerError,
                ResponseConstants.ERROR_EXECUTING);
        }

        private static Func<UpdateDefinitionBuilder<Order>, UpdateDefinition<Order>> PrepareUpdateDefinition(
            ChangeOrderStatusCommand request)
        {
            Func<UpdateDefinitionBuilder<Order>, UpdateDefinition<Order>> update;
            if (request.Status == nameof(OrderStatus.Success))
            {
                update = q =>
                    q.Set(x => x.Status, request.Status)
                        .Set(x => x.IsPaid, true)
                        .Set(x => x.CheckoutAt, DateTime.UtcNow);
            }
            else
            {
                update = q => q.Set(x => x.Status, request.Status);
            }

            return update;
        }

        private static bool CheckConstraintStatusUpdate(ChangeOrderStatusCommand request, Order order)
        {
            switch (request.Status)
            {
                case nameof(OrderStatus.Delivered):
                    if (order.Status != nameof(OrderStatus.Pending))
                        return false;
                    break;

                case nameof(OrderStatus.Success):
                    if (order.Status != nameof(OrderStatus.Delivered))
                        return false;
                    break;

                case nameof(OrderStatus.Pending):
                    if (request.Status != nameof(OrderStatus.Canceled))
                        return true;
                    break;

                case nameof(OrderStatus.Canceled):
                    break;
            }

            return true;
        }
    }
}
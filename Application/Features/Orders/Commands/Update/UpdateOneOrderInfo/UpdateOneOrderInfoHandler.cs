using Application.Features.Objects.Commands.Update.UpdateOneObject;
using Application.Features.Orders.Vms;
using AutoMapper;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Orders.Commands.Update.UpdateOneOrderInfo
{
    public class UpdateOneOrderInfoHandler : UpdateOneObjectInfoHandler<Order, OrderVm>
    {
        public UpdateOneOrderInfoHandler(IOrderRepository _orderRepository, IMapper _mapper) :
            base(_orderRepository, _mapper)
        {

        }
    }
}
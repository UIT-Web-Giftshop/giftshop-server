using Application.Features.Objects.Commands.AddOneObject;
using Application.Features.Orders.Vms;
using AutoMapper;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Orders.Commands
{
    public class AddOneOrderHandler : AddOneObjectHandler<Order, OrderVm>
    {
        public AddOneOrderHandler(IOrderRepository _orderRepository, IMapper _mapper) :
            base(_orderRepository, _mapper)
        {

        }
    }
}
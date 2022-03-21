using Application.Features.Objects.Commands.Delete.DeleteOneObject;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Orders.Commands.Delete.DeleteOneOrder
{
    public class DeleteOneOrderHandler : DeleteOneObjectHandler<Order>
    {
        public DeleteOneOrderHandler(IOrderRepository _orderRepository) : base(_orderRepository)
        {

        }
    }
}
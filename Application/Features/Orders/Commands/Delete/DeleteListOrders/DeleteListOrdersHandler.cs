using Application.Features.Objects.Commands.Delete.DeleteListObjects;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;

namespace Application.Features.Orders.Commands.Delete.DeleteListOrders
{
    public class DeleteListOrdersHandler : DeleteListObjectsHandler<Order>
    {
        public DeleteListOrdersHandler(IOrderRepository _orderRepository) : base(_orderRepository)
        {

        }
    }
}
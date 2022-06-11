using System.Threading.Tasks;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAccessorService _accessorService;

        public OrderService(IOrderRepository orderRepository, IAccessorService accessorService)
        {
            _orderRepository = orderRepository;
            _accessorService = accessorService;
        }

        public Task<bool> MakeOrder(Order order)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CancelOrder(Order order)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ChangeOrderStatus(Order order, OrderStatus status)
        {
            throw new System.NotImplementedException();
        }
    }
}
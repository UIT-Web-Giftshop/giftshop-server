using System.Threading.Tasks;
using Domain.Entities.Order;

namespace Infrastructure.Interfaces.Services
{
    public interface IOrderService
    {
        Task<bool> MakeOrder(Order order);
        Task<bool> CancelOrder(Order order);
        Task<bool> ChangeOrderStatus(Order order, OrderStatus status);
    }
}
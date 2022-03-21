using Application.Features.Objects.Commands.Delete.DeleteOneObject;
using Domain.Entities.Order;

namespace Application.Features.Orders.Commands.Delete.DeleteOneOrder
{
    public abstract class DeleteOneOrderCommand : DeleteOneObjectCommand<Order>
    {

    }
}
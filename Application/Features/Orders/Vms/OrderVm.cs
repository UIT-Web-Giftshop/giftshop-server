using System.Collections.Generic;
using Application.Features.Objects.Vms;
using Domain.Entities.Order;

namespace Application.Features.Orders.Vms
{
    public class OrderVm : ObjectVm
    {
        public string UserId { get; set; }

        public List<OrderItem> Items { get; set; }

        public OrderPromotion Promotion { get; set; }

        public double TotalPrice { get; set; }

        public bool IsPaid { get; set; }

        public OrderStatus Status { get; set; }
    }
}
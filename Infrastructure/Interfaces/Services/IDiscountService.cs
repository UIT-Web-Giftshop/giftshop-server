using System;
using System.Threading.Tasks;
using Domain.Entities.Order;

namespace Infrastructure.Interfaces.Services
{
    public interface IDiscountService
    {
        Task GenerateCoupon(float percent, string eventCode, DateTime from, DateTime to, int count = 1);

        // calc order and delete coupon after use
        void ApplyDiscount(Order order, string couponCode);
    }
}
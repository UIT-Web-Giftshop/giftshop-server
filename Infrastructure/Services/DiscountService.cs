using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Order;
using Infrastructure.Interfaces.Repositories;
using Infrastructure.Interfaces.Services;

namespace Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ICouponRepository _couponRepository;

        public DiscountService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public Task GenerateCoupon(float percent, string eventCode, DateTime from, DateTime to, int count = 1)
        {
            var coupon = new Coupon
            {
                DiscountPercent = percent,
                EventCode = eventCode,
                ValidFrom = from,
                ValidTo = to,
                CreatedAt = DateTime.UtcNow
            };

            if (count == 1)
            {
                var task = _couponRepository.InsertAsync(coupon);
                task.Wait();
                return Task.FromResult(task);
                // await _couponRepository.InsertAsync(coupon);
            }

            var coupons = new List<Coupon>();
            for (var i = 0; i < count; i++)
            {
                coupons.Add(new Coupon()
                {
                    DiscountPercent = percent,
                    EventCode = eventCode,
                    ValidFrom = from,
                    ValidTo = to,
                    CreatedAt = DateTime.UtcNow
                });
            }

            var taskMany = _couponRepository.InsertManyAsync(coupons);
            taskMany.Wait();
            return Task.FromResult(taskMany);
            // await _couponRepository.InsertManyAsync(coupons);
        }


        public void ApplyDiscount(Order order, string couponCode)
        {
            order.TotalPaid = order.TotalPrice;
            
            var couponTask = _couponRepository.GetOneAsync(couponCode);
            couponTask.Wait();

            var coupon = couponTask.Result;
            if (coupon == null)
                return;
            if (!coupon.IsValid())
                return;

            order.PromotionPercent = coupon.DiscountPercent;
            order.TotalPaid -= order.TotalPrice * order.PromotionPercent / 100;

            var deleteCouponTask = _couponRepository.DeleteOneAsync(couponCode);
            deleteCouponTask.Wait();
        }
    }
}
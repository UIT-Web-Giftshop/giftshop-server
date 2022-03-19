using System;

namespace Domain.Entities.Order
{
    public static class OrderStatusExtension
    {
        /// <summary>
        /// Get the OrderStatus name
        /// </summary>
        /// <param name="status">OrderStatus type</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string GetOrderStatus(this OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => nameof(OrderStatus.Pending),
                OrderStatus.Delivered => nameof(OrderStatus.Delivered),
                OrderStatus.Success => nameof(OrderStatus.Success),
                OrderStatus.Canceled => nameof(OrderStatus.Canceled),
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
        }
    }
}
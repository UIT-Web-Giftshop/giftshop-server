using Domain.Entities.Order;
using Xunit;

namespace Web.Tests
{
    [Collection("OrderDomain")]
    public class DomainOrderTest
    {
        [Fact]
        [Trait("Category", "OrderStatus")]
        public void GetOrderStatusName_GivenOrderStatus()
        {
            const OrderStatus orderStatus = OrderStatus.Delivered;
            Assert.Equal(nameof(OrderStatus.Delivered), orderStatus.GetString());
        }

        [Fact]
        [Trait("Category", "OrderStatus")]
        public void CompareOrderStatus_GivenOrderStatus_ReturnTrue()
        {
            const OrderStatus orderStatus = OrderStatus.Success;
            var result = orderStatus.GetString() == nameof(OrderStatus.Success);
            Assert.True(result);
        }
    }
}
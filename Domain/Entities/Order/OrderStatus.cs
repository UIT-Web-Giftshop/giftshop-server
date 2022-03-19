namespace Domain.Entities.Order
{
    /// <summary>
    /// Call GetOrderStatus() to get the status name.
    /// See Web.Tests for example.
    /// </summary>
    public enum OrderStatus : int
    {
        Pending = 0,
        Delivered = 1,
        Success = 2,
        Canceled = 3,
    }
}
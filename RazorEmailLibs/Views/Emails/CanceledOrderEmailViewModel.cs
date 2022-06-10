using Domain.Entities.Order;

namespace RazorEmailLibs.Views.Emails
{
    public class CanceledOrderEmailViewModel
    {
        public Order Order { get; set; }
        public string Reason { get; set; }

        public CanceledOrderEmailViewModel(Order order, string reason)
        {
            Order = order;
            Reason = reason;
        }
    }
}
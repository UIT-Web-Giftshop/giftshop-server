using Domain.Entities.Order;

namespace RazorEmailLibs.Views.Emails
{
    public class ReceiptEmailViewModel
    {
        public ReceiptEmailViewModel(Order order)
        {
            Order = order;
        }

        public Order Order { get; set; }
    }
}
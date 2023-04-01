using Lydian.Entities.Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Business.Abstract
{
    public interface IOrderService
    {
        public Order CreateOrderFromCart(int userId);
        public IEnumerable<Order> GetOrdersByUserId(int userId);
        public Order GetOrder(int orderId);
        public IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId);
        public List<SessionLineItemOptions> CreateLineItemsForPayment(List<OrderItem> orderItems);
        public Payment AddPaymentToDatabase(Payment payment);
        public void CompletePayment(int paymentId);
        public void UpdateOrderPaymentStatus(int orderId, bool newStatus);
        public object ConfirmPayment(string code);
    }
}

using Lydian.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Data.Abstract
{
    public interface IOrderRepository
    {
        public Order CreateOrder(Order order);
        public OrderItem CreateOrderItem(OrderItem orderItem);
        public Order GetOrder(int orderId);
        public IEnumerable<Order> GetOrdersByUserId(int userId);
        public IEnumerable<OrderItem> GetOrderItems(int orderId);
        public Payment CreatePayment(Payment payment);
        public void CompletePayment(int paymentId);
        public void UpdateOrderPaymentStatus(int orderId, bool newStatus);
        public Payment GetPaymentByCode(string code);
    }
}

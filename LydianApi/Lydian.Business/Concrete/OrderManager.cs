using Lydian.Business.Abstract;
using Lydian.Data.Abstract;
using Lydian.Entities.Models;
using Microsoft.Data.SqlClient;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lydian.Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly IOrderRepository _orderRepository;
        public OrderManager(ICartService cartService, IOrderRepository orderRepository)
        {
            _cartService = cartService;
            _orderRepository = orderRepository;
        }


        public Order CreateOrderFromCart(int userId)
        {
            var cart = _cartService.GetCartByUserId(userId);
            var cartItems = _cartService.GetCartItemsByCartId(cart.CartId, true);
            var totalPrice = _cartService.CalculateTotalPriceCartItems(cartItems);

            if(cartItems.Count() == 0)
            {
                throw new Exception("Cart is empty");
            }

            var newOrder = new Order()
            {
                UserId = cart.UserId,
                ItemCount = cartItems.Count(),
                TotalPrice = totalPrice,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var createdOrder = _orderRepository.CreateOrder(newOrder);

            var orderItems = new List<OrderItem>();
            foreach(var item in cartItems)
            {
                var newOrderItem = new OrderItem()
                {
                    OrderId = createdOrder.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                var createdOrderItem = _orderRepository.CreateOrderItem(newOrderItem);
                orderItems.Add(createdOrderItem);              
            }
            _cartService.ResetCart(cart.CartId);

            return newOrder;
        }

        public Order GetOrder(int orderId)
        {
            var order = _orderRepository.GetOrder(orderId);
            return order;
        }

        public IEnumerable<Order> GetOrdersByUserId(int userId)
        {
            var orders = _orderRepository.GetOrdersByUserId(userId);
            return orders;
        }

        public IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId)
        {
            var orderItems = _orderRepository.GetOrderItems(orderId);
            return orderItems;
        }

        public List<SessionLineItemOptions> CreateLineItemsForPayment(List<OrderItem> orderItems)
        {
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in orderItems)
            {
                var newLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = (decimal)item.Product.Price * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ProductName,
                            Images = new List<string>() { "https://localhost:44301/ProductImages/" + item.Product.Image },
                            Description = item.Product.Description
                        },
                    },
                    Quantity = item.Quantity,
                };
                lineItems.Add(newLineItem);
            }
            return lineItems;
        }


        public Payment AddPaymentToDatabase(Payment payment)
        {
            var response = _orderRepository.CreatePayment(payment);
            return response;
        }

        public void CompletePayment(int paymentId)
        {
            _orderRepository.CompletePayment(paymentId);
        }

        public void UpdateOrderPaymentStatus(int orderId, bool newStatus)
        {
            _orderRepository.UpdateOrderPaymentStatus(orderId, newStatus);
        }

        public object ConfirmPayment(string code)
        {
            var payment = _orderRepository.GetPaymentByCode(code);
            if(payment == null)
            {
                throw new Exception("Can not find payment");
            }

            _orderRepository.CompletePayment(payment.PaymentId);
            _orderRepository.UpdateOrderPaymentStatus(payment.OrderId, true);
            return payment;
        }





    }
}

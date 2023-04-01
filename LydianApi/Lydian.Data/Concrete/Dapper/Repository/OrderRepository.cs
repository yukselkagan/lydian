using Dapper;
using Lydian.Data.Abstract;
using Lydian.Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Data.Concrete.Dapper.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MsSQL");
        }


        public Order CreateOrder(Order order)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Orders (UserId, ItemCount, TotalPrice, CreatedAt, UpdatedAt) OUTPUT Inserted.OrderId  " +
                    " VALUES (@userId, @itemCount, @totalPrice, @createdAt, @updatedAt) ";
                int orderId = connection.QuerySingle<int>(sql, order);
                order.OrderId = orderId;
                return order;
            }
        }


        public OrderItem CreateOrderItem(OrderItem orderItem)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO OrderItems (OrderId, ProductId, Quantity) OUTPUT Inserted.OrderItemId VALUES (@orderId, @productId, @quantity)";
                int orderItemId = connection.QuerySingle<int>(sql, orderItem);
                orderItem.OrderItemId = orderItemId;
                return orderItem;
            }
        }

        public IEnumerable<Order> GetOrdersByUserId(int userId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Orders WHERE UserId=@userId";
                var orders = connection.Query<Order>(sql, new { userId = userId });
                return orders.ToList();
            }
        }


        public Order GetOrder(int orderId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Orders WHERE OrderId=@orderId";
                var order = connection.QueryFirstOrDefault<Order>(sql, new { orderId = orderId });
                return order;
            }
        }

        public IEnumerable<OrderItem> GetOrderItems(int orderId)
        {
            using(var connection = new SqlConnection(_connectionString))
            { 
                var sql = @"SELECT * FROM OrderItems oi INNER JOIN Products p ON p.ProductId = oi.ProductId WHERE oi.OrderId = @orderId";
                var orderItems = connection.Query<OrderItem, Product, OrderItem>(sql, (orderItem, product) => {
                    orderItem.Product = product;
                    return orderItem;
                },
                splitOn: "ProductId", param: new { orderId = orderId });
                return orderItems.ToList();
            }
        }


        public Payment CreatePayment(Payment payment)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Payments (OrderId, PaymentStatus, Code) OUTPUT Inserted.PaymentId VALUES (@orderId, @paymentStatus, @code) ";
                int paymentId = connection.QuerySingle<int>(sql, payment);
                payment.PaymentId = paymentId;
                return payment;
            }
        }

        public Payment GetPaymentByCode(string code)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Payments WHERE Code=@code ";
                var payment = connection.QueryFirstOrDefault<Payment>(sql, new { code = code });
                return payment;
            }
        }

        public void CompletePayment(int paymentId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE Payments SET PaymentStatus=1 WHERE PaymentId=@paymentId ";
                connection.Execute(sql, new { paymentId = paymentId});
            }
        }

        public void UpdateOrderPaymentStatus(int orderId, bool newStatus)
        {
            int newStatusForDb = newStatus ? 1 : 0;
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE Orders SET PaymentStatus=@paymentStatus WHERE OrderId=@orderId ";
                connection.Execute(sql, new { paymentStatus = newStatusForDb, orderId = orderId });
            }
        }





    }
}

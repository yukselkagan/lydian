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
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString;
        public CartRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MsSQL");
        }


        public int CreateInitialCart(Cart cart)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Carts (UserId) OUTPUT Inserted.CartId VALUES (@userId)";
                int cartId = connection.QuerySingle<int>(sql, new { userId = cart.UserId });
                return cartId;
            }
        }

        public Cart GetCartByUserId(int userId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Carts WHERE UserId = @userId";
                var cart = connection.QueryFirstOrDefault<Cart>(sql, new { userId = userId });
                return cart;
            }
        }

        public CartItem AddCartItem(CartItem cartItem)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO CartItems (CartId, ProductId, Quantity, CreatedAt, UpdatedAt) VALUES (@cartId, @productId, @quantity, @createdAt, @updatedAt)";
                connection.Execute(sql, cartItem);
                return cartItem;
            }
        }

        public CartItem UpdateCartItem(CartItem cartItem)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE CartItems SET Quantity = @quantity WHERE CartItemId = @cartItemId ";
                connection.Execute(sql, cartItem);
                return cartItem;
            }
        }

        public CartItem GetExistingCartItem(CartItem cartItem)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM CartItems WHERE CartId = @cartId AND ProductId = @productId ";
                var existingItem = connection.QueryFirstOrDefault<CartItem>(sql, cartItem);
                return existingItem;
            }         
        }

        public IEnumerable<CartItem> GetCartItemsByCartId(int cartId, bool includeProduct = false)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                if(includeProduct == true)
                {
                    var sql = @"SELECT * FROM CartItems c INNER JOIN Products p ON p.ProductId = c.ProductId WHERE c.CartId = @cartId";
                    var cartItems = connection.Query<CartItem, Product, CartItem>(sql, (cartItem, product) => {
                        cartItem.Product = product;
                        return cartItem;
                    },
                    splitOn: "ProductId", param: new { cartId = cartId });
                    return cartItems.ToList();
                }
                else
                {
                    var sql = "SELECT * FROM CartItems WHERE CartId = @cartId ";
                    var cartItems = connection.Query<CartItem>(sql, new { cartId = cartId });
                    return cartItems.ToList();
                }
            }
        }

        public void DeleteCartItem(CartItem cartItem)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM CartItems WHERE CartItemId = @cartItemId ";
                connection.Execute(sql, cartItem);
            }
        }



    }
}

using Lydian.Business.Abstract;
using Lydian.Data.Abstract;
using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Business.Concrete
{
    public class CartManager : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartManager(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }



        public int CreateInitialCart(User user)
        {
            var newCart = new Cart()
            {
                UserId = user.UserId
            };

            var cartId = _cartRepository.CreateInitialCart(newCart);
            return cartId;
        }

        public CartItem AddItemToCart(int userId, CartItemAddDto cartItemAddDto)
        {
            var cart = GetCartByUserId(userId);

            var newCartItem = new CartItem()
            {
                CartId = cart.CartId,
                ProductId = cartItemAddDto.ProductId,
                Quantity = cartItemAddDto.Quantity,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var existingCartItem = _cartRepository.GetExistingCartItem(newCartItem);

            CartItem response = null;
            if(existingCartItem == null)
            {
                response = _cartRepository.AddCartItem(newCartItem);
            }
            else
            {
                existingCartItem.Quantity += cartItemAddDto.Quantity;
                response = _cartRepository.UpdateCartItem(existingCartItem);
            }
         
            return response;
        }

        public Cart GetCartByUserId(int userId)
        {
            var cart = _cartRepository.GetCartByUserId(userId);
            return cart;
        }

        public IEnumerable<CartItem> GetCartItemsByCartId(int cartId, bool includeProduct = false)
        {
            var cartItems = _cartRepository.GetCartItemsByCartId(cartId, includeProduct);
            return cartItems;
        }

        public double CalculateTotalPriceCartItems(IEnumerable<CartItem> cartItems)
        {
            double totalPrice = 0;
            foreach (var item in cartItems)
            {
                totalPrice += item.Product.Price * item.Quantity;
            }
            return totalPrice;
        }

        public void ResetCart(int cartId)
        {
            var cartItems = GetCartItemsByCartId(cartId);
            foreach(var item in cartItems)
            {
                _cartRepository.DeleteCartItem(item);
            }
        }


    }
}

using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Business.Abstract
{
    public interface ICartService
    {
        public int CreateInitialCart(User user);
        public Cart GetCartByUserId(int userId);
        public IEnumerable<CartItem> GetCartItemsByCartId(int cartId, bool includeProduct = false);
        public CartItem AddItemToCart(int userId, CartItemAddDto cartItemAddDto);
        public double CalculateTotalPriceCartItems(IEnumerable<CartItem> cartItems);
        public void ResetCart(int cartId);

    }
}

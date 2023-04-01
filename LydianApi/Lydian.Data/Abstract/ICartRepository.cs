using Lydian.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Data.Abstract
{
    public interface ICartRepository
    {
        public int CreateInitialCart(Cart cart);
        public Cart GetCartByUserId(int userId);
        public CartItem AddCartItem(CartItem cartItem);
        public CartItem UpdateCartItem(CartItem cartItem);
        public CartItem GetExistingCartItem(CartItem cartItem);
        public IEnumerable<CartItem> GetCartItemsByCartId(int cartId, bool includeProduct = false);
        public void DeleteCartItem(CartItem cartItem);

    }
}

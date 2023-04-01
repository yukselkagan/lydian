using Lydian.Business.Abstract;
using Lydian.Entities.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace Lydian.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        public CartController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }


        [HttpPost("cart-item")]
        public ActionResult AddItemToCart(CartItemAddDto cartItemAddDto)
        {
            try
            {
                ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _userService.ReadUserToken(claimsIdentity);

                var response = _cartService.AddItemToCart(userId, cartItemAddDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult GetCartWithItems()
        {
            try
            {
                ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _userService.ReadUserToken(claimsIdentity);

                var cart = _cartService.GetCartByUserId(userId);
                var cartItems = _cartService.GetCartItemsByCartId(cart.CartId, true);
                cart.CartItems = cartItems.ToList();

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }






    }
}

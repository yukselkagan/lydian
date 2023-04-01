using Lydian.Business.Abstract;
using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lydian.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        public OrderController(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;

            StripeConfiguration.ApiKey = "My stripe key";
        }

        [HttpGet]
        public ActionResult GetOrders()
        {
            try
            {
                ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _userService.ReadUserToken(claimsIdentity);

                var orders = _orderService.GetOrdersByUserId(userId);
                foreach (var item in orders)
                {
                    item.OrderItems = _orderService.GetOrderItemsByOrderId(item.OrderId).ToList();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public ActionResult CreateOrderFromCart()
        {
            try
            {
                ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _userService.ReadUserToken(claimsIdentity);

                var response = _orderService.CreateOrderFromCart(userId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("payment")]
        public ActionResult CreatePayment(PaymentCreateDto paymentCreateDto)
        {
            try
            {
                ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _userService.ReadUserToken(claimsIdentity);

                var order = _orderService.GetOrder(paymentCreateDto.OrderId);
                var orderItems = _orderService.GetOrderItemsByOrderId(order.OrderId);
                if(order == null)
                {
                    throw new Exception("Can not find order");
                }
                if(order.PaymentStatus == true)
                {
                    throw new Exception("AlreadyPaid");
                }

                var lineItems = _orderService.CreateLineItemsForPayment(orderItems.ToList());
                var newPayment = new Payment() { OrderId = order.OrderId };
                newPayment = _orderService.AddPaymentToDatabase(newPayment);

                var options = new SessionCreateOptions
                {
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = $"http://localhost:4200/payment-success?id={newPayment.Code}",
                    CancelUrl = "http://localhost:4200/payment-cancel",
                };

                var service = new SessionService();
                Session session = service.Create(options);

                Response.Headers.Add("Location", session.Url);
                var response = new CommonResponse() { Data = session.Url };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new CommonResponse() { Data = ex.Message } );
            }
        }


        [HttpPost("payment/confirm/{code}")]
        public ActionResult ConfirmPayment(string code)
        {
            try
            {
                ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _userService.ReadUserToken(claimsIdentity);

                _orderService.ConfirmPayment(code);

                return Ok(new CommonResponse() { Data = "PaymentConfirmed"} );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        



        [HttpGet("PayTest")]
        [AllowAnonymous]
        public ActionResult PayTest()
        {
            


            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = 2000,
                      Currency = "usd",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {                       
                        Name = "T-shirt",
                        Images = new List<string>() { "https://www.oyupa.com/assets/images/cover-tradesman.jpg" },
                        Description = "About product"
                      },
                    },
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:4242/success",
                CancelUrl = "http://localhost:4242/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            //return new StatusCodeResult(303);



            return Ok(session.Url);
        }



    }
}

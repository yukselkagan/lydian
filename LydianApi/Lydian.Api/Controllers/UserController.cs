using AutoMapper;
using Lydian.Business.Abstract;
using Lydian.Entities.Base;
using Lydian.Entities.Dto;
using Lydian.Entities.IBase;
using Lydian.Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lydian.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public UserController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public string GetAll()
        {
            return "hello user";
        }



        [HttpPost("Register")]
        [AllowAnonymous]
        public ActionResult<UserToken> Register(UserRegisterDto userRegisterDto)
        {
            try
            {
                var responseToken = _service.Register(userRegisterDto);
                return responseToken;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }        
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public ActionResult<UserToken> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var responseToken = _service.Login(userLoginDto);
                return Ok(responseToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }         
        }

        [HttpGet("GetUserSelf")]
        [AllowAnonymous]
        public ActionResult GetUserSelf()
        {
            try
            {
                ClaimsIdentity claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                int userId = _service.ReadUserToken(claimsIdentity);

                if (userId > 0)
                {
                    var responseUser = _service.GetUser(userId);
                    var userDto = _mapper.Map<UserDto>(responseUser);
                    return Ok(userDto);
                }
                else
                {
                    throw new Exception("Can not read token");
                }
            }
            catch (Exception ex)
            {
                var response = new ErrorResponse(ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public ActionResult RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            try
            {
                var responseToken = _service.RefreshToken(refreshTokenRequestDto);
                return Ok(responseToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
        }




    }
}

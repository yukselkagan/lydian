using AutoMapper;
using Lydian.Business.Abstract;
using Lydian.Business.General;
using Lydian.Business.General.Token;
using Lydian.Data.Abstract;
using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        IConfiguration _configuration;
        IServiceProvider _serviceProvider;
        public UserManager(IUserRepository repository, IMapper mapper, 
            IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public UserToken Register(UserRegisterDto userRegisterDto)
        {
            var mappedUser = _mapper.Map<User>(userRegisterDto);
            mappedUser.PasswordHash = PasswordManager.CreatePasswordHash(userRegisterDto.Password);

            var responseUser = _repository.Register(mappedUser);
            var cartService = _serviceProvider.GetRequiredService<ICartService>();
            var cartId = cartService.CreateInitialCart(responseUser);
            _repository.AssignInitialCart(responseUser.UserId, cartId);

            var tokenString = new TokenManager(_configuration).CreateToken(responseUser);

            var userToken = new UserToken
            {
                User = responseUser,
                AccessToken = tokenString
            };

            return userToken;
        }

        public UserToken Login(UserLoginDto userLoginDto)
        {
            var mappedUser = _mapper.Map<User>(userLoginDto);
            mappedUser.PasswordHash = PasswordManager.CreatePasswordHash(userLoginDto.Password);

            var responseUser = _repository.Login(mappedUser);

            if(responseUser is not null)
            {
                var tokenManager = new TokenManager(_configuration);
                var accesstoken = tokenManager.CreateToken(responseUser);
                var refreshToken = tokenManager.CreateRefreshToken();

                _repository.UpdateRefreshToken(new UserRefreshTokenDto()
                {
                    UserId = responseUser.UserId,
                    RefreshToken = refreshToken,
                    RefreshTokenExpirationTime = DateTime.Now.AddDays(5)
                });

                var userToken = new UserToken
                {
                    User = responseUser,
                    AccessToken = accesstoken,
                    RefreshToken = refreshToken
                };

                return userToken;
            }
            else
            {
                throw new Exception("Email or password is wrong");
            }
        }


        public int ReadUserToken(ClaimsIdentity claimsIdentity)
        {
            List<Claim> claims = claimsIdentity.Claims.ToList();
            Claim claimOfId = claims.FirstOrDefault(x => x.Type == "id");

            int userId;
            if(claimOfId != null)
            {
                int.TryParse(claimOfId.Value, out userId);
                return userId;
            }
            else
            {
                throw new Exception("Invalid token");
            }        
        }

        public User GetUser(int userId)
        {
            var responseUser = _repository.GetUser(userId);
            return responseUser;
        }

        public UserToken RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            var tokenManager = new TokenManager(_configuration);

            var claimsPrincipal = tokenManager.GetPrincipalFromExpiredToken(refreshTokenRequestDto.AccessToken);
            var claimOfIdValue = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "id").Value;

            int.TryParse(claimOfIdValue, out int userId);
            if (userId > 0)
            {
                var user = _repository.GetUser(userId);
                if(user.RefreshToken == refreshTokenRequestDto.RefreshToken)
                {
                    if(user.RefreshTokenExpirationTime <= DateTime.Now)
                    {
                        throw new Exception("Expired refresh token");
                    }

                    var newAccessToken = tokenManager.CreateToken(user);

                    var userToken = new UserToken
                    {
                        User = user,
                        AccessToken = newAccessToken
                    };
                    return userToken;
                }
                else
                {
                    throw new Exception("Invalid refresh token");
                }               
            }
            else
            {
                throw new Exception("Can not read token");
            }
        }




    }
}

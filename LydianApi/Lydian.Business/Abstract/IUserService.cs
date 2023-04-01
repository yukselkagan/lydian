using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Business.Abstract
{
    public interface IUserService
    {
        public UserToken Register(UserRegisterDto userRegisterDto);
        public UserToken Login(UserLoginDto userLoginDto);
        public int ReadUserToken(ClaimsIdentity claimsIdentity);
        public User GetUser(int userId);
        public UserToken RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto);
    }
}

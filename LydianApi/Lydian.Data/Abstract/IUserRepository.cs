using Lydian.Entities.Dto;
using Lydian.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Data.Abstract
{
    public interface IUserRepository
    {
        public User Register(User userRegister);
        public User Login(User userLogin);
        public User GetUser(int userId);
        public void UpdateRefreshToken(UserRefreshTokenDto userRefreshTokenDto);
        public void AssignInitialCart(int userId, int cartId);
    }
}

using Dapper;
using Lydian.Data.Abstract;
using Lydian.Entities.Dto;
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
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MsSQL");
        }

        public User GetUser(int userId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Users WHERE UserId=@userId";
                var user = connection.QuerySingle<User>(sql, new { userId = userId });
                return user;
            }
        }

        public User Login(User userLogin)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Users WHERE Email = @email AND PasswordHash = @passwordHash ";
                var user = connection.QueryFirstOrDefault<User>(sql, userLogin);
                return user;
            }
        }

        public User Register(User userRegister)
        {
            var haveSameEmail = ControlExistingEmail(userRegister.Email);
            if(haveSameEmail)
            {
                throw new Exception("Email already taken");
            }

            using(var connection = new SqlConnection(_connectionString))
            {
                //var sql = "INSERT INTO Users (Email, Password) VALUES (@email, @password)";
                //connection.Execute(sql, userRegister);

                var sql = "INSERT INTO Users (Email, PasswordHash) OUTPUT Inserted.UserId  VALUES (@email, @passwordHash) ";               
                int userId = connection.QuerySingle<int>(sql, userRegister);
                userRegister.UserId = userId;

                return userRegister;
            }
        }

        public void UpdateRefreshToken(UserRefreshTokenDto userRefreshTokenDto)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE Users SET RefreshToken = @refreshToken, RefreshTokenExpirationTime = @refreshTokenExpirationTime  WHERE UserId=@userId ";
                connection.Execute(sql, userRefreshTokenDto);
            }
        }

        private bool ControlExistingEmail(string email)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT COUNT(*) FROM Users WHERE Email = @email ";
                var resultCount = connection.ExecuteScalar<int>(sql, new {email = email });
                if(resultCount == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        public void AssignInitialCart(int userId, int cartId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                var sql = "UPDATE Users SET CartId = @cartId WHERE UserId=@userId ";
                connection.Execute(sql, new { cartId = cartId, userId = userId });
            }
        }


    }
}

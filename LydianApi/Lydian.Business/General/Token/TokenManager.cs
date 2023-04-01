using Lydian.Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lydian.Business.General.Token
{
    public class TokenManager
    {
        IConfiguration _configuration;
        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("id", user.UserId.ToString()),
                new Claim("email", user.Email)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                    claims: claimsIdentity.Claims,

                    issuer : _configuration["Tokens:Issuer"],
                    audience : _configuration["Tokens:Audience"],
                    
                    signingCredentials : signingCredentials,

                    expires: DateTime.Now.AddMinutes(50),
                    notBefore: DateTime.Now
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string createdTokenString = tokenHandler.WriteToken(token);

            return createdTokenString;
        }


        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _configuration["Tokens:Audience"],
              
                ValidateIssuer = true,
                ValidIssuer = _configuration["Tokens:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
                
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }




    }
}

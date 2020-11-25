using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace DPTPWebAPI.AxisSetuAPI.Model
{
    public class JwtAuthManager
    {
        static string SecretKey = "75c7a355-863b-442e-9e05-f64b7da395cc"; // this should come from your configuration file
        static string audience = "28e836ea-ada9-4c93-8234-e3c498aa598a";// this should come from your configuration file
        public string GenerateJWTToken()
        {
            byte[] symmetricKey = Encoding.ASCII.GetBytes(JwtAuthManager.SecretKey);
            var token_Handler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var securitytokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = JwtAuthManager.audience,
                IssuedAt = now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256)
            };
            var stoken = token_Handler.CreateToken(securitytokenDescriptor);
            var token = token_Handler.WriteToken(stoken);
            return token;
        }
        public bool ValidateToken(string token)
        {
            var simplePrinciple = JwtAuthManager.GetPrincipal(token);
            Console.WriteLine(simplePrinciple);
            if (simplePrinciple == null)
                return false;
            // You can implement more validation to check whether username exists in your DB or not or something else. 
            return true;
        }
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtTokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                    return null;
                byte[] symmetricKey = Encoding.ASCII.GetBytes(JwtAuthManager.SecretKey);
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    RequireExpirationTime = false,
                    ValidateIssuer = false,
                    ValidAudience = JwtAuthManager.audience,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };
                SecurityToken securityToken;
                var principal = jwtTokenHandler.ValidateToken(token, validationParameters, out securityToken);
                Console.WriteLine("Principle");
                Console.WriteLine(principal);
                return principal;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }

}
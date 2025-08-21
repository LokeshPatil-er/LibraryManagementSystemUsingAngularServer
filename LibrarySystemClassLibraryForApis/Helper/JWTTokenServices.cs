using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemClassLibraryForApis
{
    public class JWTTokenServices
    {
        private static readonly string key = ConfigurationManager.AppSettings["secretKey"];

        public string TokenGenertor(string userEmail)
        {
            string token = "";
            try
            {
                int expiry =Convert.ToInt32( ConfigurationManager.AppSettings["expiryTime"]);
                var secretKey = Encoding.UTF8.GetBytes(key);
                var tokenHandler = new JwtSecurityTokenHandler();

                var descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, userEmail)
                }),
                    Expires = DateTime.UtcNow.AddMinutes(expiry),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                };

                 token =  tokenHandler.WriteToken(tokenHandler.CreateToken(descriptor));
               
            }
            catch(Exception ex)
            {

            }
            return token;
        }

        public static ClaimsPrincipal TokenValidation(string token)
        {
            try
            {
                 var secretKey = Encoding.UTF8.GetBytes(key);

                var tokenParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuerSigningKey = true
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken securityToken;
                var claimPrinciple = tokenHandler.ValidateToken(token, tokenParameters, out securityToken);
                return claimPrinciple;

            }
            catch (Exception ex)
            {
                
                return null;
            }
        }
    }
}

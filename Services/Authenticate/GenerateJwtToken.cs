using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Domain.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Services.Authenticate
{
    public static class GenerateJwtToken
    {
        public static string GenerateUserJwtToken(this IConfiguration configuration, 
                                                  UserAuthenticateResponse userAuthenticateResponse)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            //берём ключ из appsettings.json
            var key = Encoding.ASCII.GetBytes(configuration["JwtConfig:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //сущность, описывающая требования для предоставления аутентификации
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("id", userAuthenticateResponse.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userAuthenticateResponse.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userAuthenticateResponse.Role.ToString())
                }),

                //срок действия токена
                Expires = DateTime.UtcNow.AddMinutes(5),

                //установка ключа и алгоритма для создания токена
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}

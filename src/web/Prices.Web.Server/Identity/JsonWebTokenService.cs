using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Prices.Web.Server.Identity
{
    public interface ITokenService
    {
        string BuildToken(string email);
    }

    public class JsonWebTokenService : ITokenService
    {
        private readonly JsonWebTokenConfiguration _configuration;
        private readonly SecurityTokenHandler _tokenHandler;

        public JsonWebTokenService(JsonWebTokenConfiguration configuration, SecurityTokenHandler tokenHandler)
        {
            _configuration = configuration;
            _tokenHandler = tokenHandler;
        }

        public string BuildToken(string email)
        {
            return _tokenHandler.WriteToken(CreateToken(email));
        }

        private JwtSecurityToken CreateToken(string email)
        {
            return new JwtSecurityToken(_configuration.Issuer, _configuration.Audience, Claims(email),
                expires: DateTime.Now.AddMinutes(_configuration.ExpiryMinutesToAdd),
                signingCredentials: Credentials());
        }

        private SigningCredentials Credentials()
        {
            return new SigningCredentials(CreateKey(), SecurityAlgorithms.HmacSha256);
        }

        private SymmetricSecurityKey CreateKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));
        }

        private static IEnumerable<Claim> Claims(string email)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }
    }
}
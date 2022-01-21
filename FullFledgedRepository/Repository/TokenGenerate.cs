using FullFledgedDto;
using FullFledgedModel;
using FullFledgedRepository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FullFledgedRepository.Repository
{
    public class TokenGenerate : ITokenInterface
    {
        private readonly IConfiguration _configuration;

        public TokenGenerate(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string TokenGenerateString(UserLogin register)
        {
            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var cred = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,register.UserName),
                new Claim(ClaimTypes.Email,register.Email),
                new Claim(ClaimTypes.Role,register.Role)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: cred);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

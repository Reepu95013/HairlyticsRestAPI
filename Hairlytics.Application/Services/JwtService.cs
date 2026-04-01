using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        //public string GenerateToken(User user)
        //{
        //    var claims = new[]
        //     {
        //        new Claim("id", user.Id.ToString()),
        //        new Claim(ClaimTypes.Name, user.Name),
        //        new Claim(ClaimTypes.Role, user.Role.ToString())
        //    };

        //    var key = new SymmetricSecurityKey(
        //             Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        //        );

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _config["Jwt:Issuer"],
        //        audience: _config["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(10),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
            
        //}


        public string GenerateToken(User user)
        {
            var jwtTokerHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("V3ryL0ngAndC0mpl3xS3cr3tK3y256df");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
                Issuer= _config["Jwt:Issuer"],
                Audience= _config["Jwt:Audience"],
            };

            var token = jwtTokerHandler.CreateToken(tokenDescriptor);

            return jwtTokerHandler.WriteToken(token);


        }
    }
}

using CarWashAPI.Models;
using CarWashAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarWashAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static User user = new User();

        [HttpPost]
        [Route("register")]
        public ActionResult<User> Register(UserDTO request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;

            string token = CreateJWTtoken(user);

            return Ok(token);
        }

        [HttpPost]
        public ActionResult<User> Login(UserDTO request)
        {
            bool isAMatch = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (user.Username != request.Username || !isAMatch)
            {
                return BadRequest("Invalid Credentials");
            }
            return Ok(user);
        }

        private string CreateJWTtoken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim (ClaimTypes.Name, user.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWTSettings:Token").Value
                ));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration.GetSection("JWTSettings:Day").Value))
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}


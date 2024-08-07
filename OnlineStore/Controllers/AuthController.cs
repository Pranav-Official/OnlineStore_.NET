
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Exceptions;
using OnlineStore.Models;
using OnlineStore.Services;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        public IAuthenticationService _authService;
        public readonly IConfiguration configuration;

        public AuthController(IAuthenticationService authService, IConfiguration configuration)
        {
            _authService = authService;
            this.configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> RegisterAsync(Users user)
        {
            try
            {
                await _authService.Register(user);
                var response = new { Status = "success" };
                return Ok(response);
            }
            catch (UserAlreadyExistsException)
            {
                return StatusCode(401, new { Status = "fail", Error = "User already Exists" });
            }
            catch (Exception)
            {
                // Log the exception if needed
                return StatusCode(500, new { Status = "fail", Error = "An unexpected error occurred." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(string userName, string password)
        {

            var User = await _authService.LoginAsync(userName, password);

            if (User.UserName != "")
            {
                var claims = new[]
                {
                    new Claim("Role", User.Role),
                    new Claim("UserName", userName),
                };
                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("uweqyrtweuqwerytqweuirytqwueyrtuweqyrtweuqwerytqweuirytqwueyrt"));
                var Signing = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

                var Token = new JwtSecurityToken(
                    "Issuer",
                    "Audiance",
                    claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: Signing
                    );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(Token);
                return Ok(new { Status = "success", Token = tokenString });
            }
            return Unauthorized(new { Status = "fail", Error = "Invalid credentials" });
        }
    }
}

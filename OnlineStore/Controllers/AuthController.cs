
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineStore.Models;
using OnlineStore.Services;
using System.Data;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        public IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService, IConfiguration configuration)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public IActionResult Register(Users user)
        {
            try
            {
                _authService.Register(user);
                var token = _authService.GenerateJwt(user.UserName, user.Password, user.Role);
                var response = new { Status = "success", Data = token };
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Status = "fail", Error = ex.Message});
            }
        }

        [HttpPost("login")]
        public IActionResult Login(string userName, string password)
        {
            if (_authService.Login(userName, password, out var role))
            {
                return Ok(new { Status = "success", Role = role });
            }
            return Unauthorized(new { Status = "fail", Error = "Invalid credentials" });
        }
    }
}

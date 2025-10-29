using Microsoft.AspNetCore.Mvc;
using ProductApi.Helpers;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(AuthService authService, JwtHelper jwtHelper)
        {
            _authService = authService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var user = _authService.Authenticate(login.Username, login.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = _jwtHelper.GenerateToken(user.Username);
            return Ok(new { token });
        }
    }
}

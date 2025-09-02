using Microsoft.AspNetCore.Mvc;
using PM.Application.UseCases.Auth;
using System.Threading.Tasks;

namespace PM.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthUseCase _authUseCase;
        public AuthController(AuthUseCase authUseCase)
        {
            _authUseCase = authUseCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authUseCase.LoginAsync(request.Email, request.Password);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();
            return Ok(new { token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var token = await _authUseCase.RefreshTokenAsync(request.RefreshToken);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();
            return Ok(new { token });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await _authUseCase.LogoutAsync(request.Token);
            return Ok(new { message = "Logged out" });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RefreshRequest
    {
        public string RefreshToken { get; set; }
    }
    public class LogoutRequest
    {
        public string Token { get; set; }
    }
}

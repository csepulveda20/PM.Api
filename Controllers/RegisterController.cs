using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PM.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterUserUseCase _registerUserUseCase;
        public RegisterController(RegisterUserUseCase registerUserUseCase)
        {
            _registerUserUseCase = registerUserUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _registerUserUseCase.Register(request.Email, request.Password, request.Role);
            return Ok(new { message = "Usuario registrado correctamente" });
        }

        public class RegisterRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }
    }
}

using System.Threading.Tasks;
using PM.Application.Repository;

namespace PM.Application.UseCases.Auth
{
    public class AuthUseCase
    {
        private readonly IAuth _authService;
        public AuthUseCase(IAuth authService)
        {
            _authService = authService;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            return await _authService.LoginAsync(email, password);
        }

        public async Task<string> RefreshTokenAsync(string refreshToken)
        {
            return await _authService.RefreshTokenAsync(refreshToken);
        }

        public async Task LogoutAsync(string token)
        {
            await _authService.LogoutAsync(token);
        }
    }
}

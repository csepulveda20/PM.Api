namespace PM.Application.Repository;

public interface IAuth
{
    Task<string> LoginAsync(string email, string password);
    Task LogoutAsync(string token); // Stub para logout, puede ser extendido para blacklist
    Task<string> RefreshTokenAsync(string refreshToken); // Retorna nuevo JWT
}
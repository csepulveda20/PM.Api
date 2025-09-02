using PM.Application.Repository;
using PM.Domain.Entities;

public class RegisterUserUseCase(IUserRepository repo, IPasswordHasher hasher)
{
    private readonly IUserRepository _repo = repo;
    private readonly IPasswordHasher _hasher = hasher;

    public async Task Register(string email, string password, string role)
    {
        var passwordHash = _hasher.HashPassword(password);
        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };
        await _repo.AddUserAsync(user);
    }
}
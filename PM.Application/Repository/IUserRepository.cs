using PM.Domain.Entities;

namespace PM.Application.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByRefreshTokenAsync(string refreshToken);

        Task AddUserAsync(User user);
    }
}

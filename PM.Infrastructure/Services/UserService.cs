using PM.Application.Repository;
using PM.Domain.Entities;
using PM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace PM.Infrastructure.Services
{
    public class UserService : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly UnitOfWork _unitOfWork;
        public UserService(DatabaseContext context, UnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task AddUserAsync(User user)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            // Implementa la lógica real si tienes refresh tokens en la entidad User
            // return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            return await Task.FromResult<User>(null);
        }
    }
}

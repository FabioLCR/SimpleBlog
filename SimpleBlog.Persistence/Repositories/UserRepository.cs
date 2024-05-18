using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Persistence.Context;

namespace SimpleBlog.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqliteContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IDatabase database, ILogger<UserRepository> logger)
        {
            _context = (SqliteContext)database;
            _logger = logger;
        }

        public async Task<UserEntity?> GetByUsername(string? username)
        {
            _logger.LogInformation("Obtendo usuário pelo nome de usuário: {Username}", username);
            return await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task Add(UserEntity user)
        {
            _logger.LogInformation("Adicionando novo usuário: {Username}", user.Username);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
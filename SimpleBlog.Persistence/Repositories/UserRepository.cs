using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Persistence.Context;

namespace SimpleBlog.Persistence.Repositories
{


    namespace SimpleBlog.Persistence.Repositories
    {
        public class UserRepository : IUserRepository
        {
            private readonly SqliteContext _context;

            public UserRepository(IDatabase database)
            {
                _context = (SqliteContext)database;
            }

            public async Task<UserEntity?> GetByUsername(string? username) => 
                await _context.Users.FirstOrDefaultAsync(user => user.Username == username);

            public async Task Add(UserEntity user)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }
    }

}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Persistence.Interfaces
{
    public interface ISqliteContext
    {
        DbSet<PostEntity>? Posts { get; set; }
        DbSet<UserEntity>? Users { get; set; }

        ValueTask<T?> FindAsync<T>(params object[] keyValues) where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Update<T>(T entity) where T : class;
    }
}
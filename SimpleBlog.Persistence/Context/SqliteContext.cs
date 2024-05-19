using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Persistence.Interfaces;

namespace SimpleBlog.Persistence.Context
{
    public class SqliteContext : DbContext, ISqliteContext
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options) { }

        public DbSet<UserEntity>? Users { get; set; }
        public DbSet<PostEntity>? Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }

        public new ValueTask<T?> FindAsync<T>(params object[] keyValues) where T : class
        {
            return base.FindAsync<T>(keyValues);
        }
        public new void Update<T>(T entity) where T : class => Entry(entity).State = EntityState.Modified;
    }
}
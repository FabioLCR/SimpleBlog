using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using System.Data.Common;

namespace SimpleBlog.Persistence.Context
{
    public class SqliteContext : DbContext, IDatabase
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PostEntity> Posts { get; set; }

    }
}
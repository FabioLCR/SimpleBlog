using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Interfaces;
using System.Data.Common;

namespace SimpleBlog.Persistence.Context
{
    public class SqliteContext : DbContext, IDatabase
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        {

        }

        public DbConnection Connection()
        {
            return Database.GetDbConnection();
        }
        
    }
}
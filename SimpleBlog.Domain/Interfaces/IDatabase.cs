using System.Data.Common;

namespace SimpleBlog.Domain.Interfaces
{
    public interface IDatabase
    {
        DbConnection Connection();
    }
}
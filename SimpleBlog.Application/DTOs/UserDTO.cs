using System.Security.Cryptography;
using System.Text;

namespace SimpleBlog.Domain.Entities
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}

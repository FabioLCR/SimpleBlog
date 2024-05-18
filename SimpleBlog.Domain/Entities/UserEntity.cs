using System.Security.Cryptography;
using System.Text;

namespace SimpleBlog.Domain.Entities
{
    public class UserEntity
    {
        private string? _password;

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password
        {
            get => _password;
            init => _password = value;
        }

        public void SetPassword(string password, string key)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(key), 10000);
            _password = Convert.ToBase64String(pbkdf2.GetBytes(20));
        }

        public bool IsPasswordValid(string password, string key)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(key), 10000);
            return Password == Convert.ToBase64String(pbkdf2.GetBytes(20));
        }


    }
}

using Microsoft.Extensions.Configuration;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;

namespace SimpleBlog.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task Register(string username, string password)
        {
            if (await _userRepository.GetByUsername(username) != null)
                throw new Exception("Usuario já existe");

            var userSecret = _configuration["UserSecret"] 
                ?? throw new Exception("UserSecret não está configurado corretamente");

            var user = new UserEntity { Username = username };
            user.SetPassword(password, userSecret);
            await _userRepository.Add(user);
        }
    }
}

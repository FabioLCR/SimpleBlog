using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public async Task<string?> Login(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);
            if (user == null)
                return null;

            var userSecret = _configuration["UserSecret"]
                ?? throw new Exception("UserSecret não está configurado corretamente");

            if (!user.IsPasswordValid(password, userSecret))
                return null;

            return GenerateJwtToken(user);
        }

        public async Task<UserEntity?> GetLoggedInUser(ClaimsPrincipal userPrincipal)
        {
            //Obtem o usuario logado
            var username = userPrincipal.Identity?.Name;
            return username != null ? await _userRepository.GetByUsername(username) : null;
        }


        private string GenerateJwtToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.Name, user.Username!.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

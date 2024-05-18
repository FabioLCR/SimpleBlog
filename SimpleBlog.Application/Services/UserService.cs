using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleBlog.Application.Services
{
    public class UserService : IUserService
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
                throw new UserAlreadyExistsException("Usuário já existe");

            var userSecret = _configuration["UserSecret"]
                ?? throw new ConfigurationException("UserSecret não está configurado corretamente");

            var user = new UserEntity { Username = username };
            user.SetPassword(password, userSecret);
            await _userRepository.Add(user);
        }

        public async Task<UserEntity> GetLoggedInUser(ClaimsPrincipal userPrincipal) =>
            await _userRepository.GetByUsername(userPrincipal.Identity?.Name)
                ?? throw new UserNotAuthorizedException("Não foi possível obter o usuário logado");

        public async Task<string?> Login(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);
            if (user == null)
                throw new UserNotFoundException("Usuário não encontrado");

            var userSecret = _configuration["UserSecret"]
                ?? throw new ConfigurationException("UserSecret não está configurado corretamente");

            if (!user.IsPasswordValid(password, userSecret))
                throw new InvalidPasswordException("Senha inválida");

            return GenerateJwtToken(user);
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

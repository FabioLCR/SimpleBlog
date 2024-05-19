using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Application.Mappers;
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
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository, 
            IConfiguration configuration, 
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Register(string username, string password)
        {
            _logger.LogInformation("Registro iniciado para o usuário: {Username}", username);

            if (await _userRepository.GetByUsername(username) != null)
                throw new UserAlreadyExistsException("Usuário já existe");

            var userSecret = _configuration["UserSecret"]
                ?? throw new ConfigurationException("UserSecret não está configurado corretamente");

            var user = new UserEntity { Username = username };
            user.SetPassword(password, userSecret);
            await _userRepository.Add(user);

            _logger.LogInformation("Registro concluído para o usuário: {Username}", username);
        }

        public async Task<UserDTO> GetLoggedInUser(ClaimsPrincipal userPrincipal)
        {
            _logger.LogInformation("Obtendo usuário logado");

            var loggedInUser = await _userRepository.GetByUsername(userPrincipal.Identity?.Name) 
                ?? throw new UserNotAuthorizedException("Usuário não autorizado");

            _logger.LogInformation("Usuário logado obtido: {Username}", loggedInUser.Username);
            return loggedInUser.ToDTO();
        }

        public async Task<string?> Login(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username) 
                ?? throw new UserNotFoundException("Usuário não encontrado");

            _logger.LogInformation("Login iniciado para o usuário: {Username}", username);

            var userSecret = _configuration["UserSecret"]
                ?? throw new ConfigurationException("UserSecret não está configurado corretamente");

            if (!user.IsPasswordValid(password, userSecret))
                throw new InvalidPasswordException("Senha inválida");

            _logger.LogInformation("Login bem-sucedido para o usuário: {Username}", username);
            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(UserEntity user)
        {
            _logger.LogInformation("Gerando token JWT para o usuário: {Username}", user.Username);

            if (_configuration["Jwt:Key"] == null)
                throw new ConfigurationException("Chave JWT não está configurada corretamente");

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

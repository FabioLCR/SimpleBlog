using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Services;

namespace SimpleBlog.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            _logger.LogInformation("Registrando usuário {Username}", username);
            try
            {
                await _userService.Register(username, password);
                _logger.LogInformation("Usuário {Username} registrado com sucesso", username);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário {Username}", username);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            _logger.LogInformation("Logando usuário {Username}", username);
            try
            {
                var token = await _userService.Login(username, password);
                if (token == null)
                {
                    _logger.LogInformation("Falha ao logar usuário {Username}", username);
                    return Unauthorized();
                }

                _logger.LogInformation("Usuário {Username} logado com sucesso", username);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao logar usuário {Username}", username);
                return BadRequest(ex.Message);
            }
        }
    }
}

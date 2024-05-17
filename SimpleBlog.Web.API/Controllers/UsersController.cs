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
    }
}

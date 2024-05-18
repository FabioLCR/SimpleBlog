using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Exceptions;

namespace SimpleBlog.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            await _userService.Register(username, password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var token = await _userService.Login(username, password);
                return Ok(new { token });
            }
            catch (UserNotFoundException) { return Unauthorized("Credenciais inválidas"); }
            catch (InvalidPasswordException) { return Unauthorized("Credenciais inválidas"); }
        }
    }
}

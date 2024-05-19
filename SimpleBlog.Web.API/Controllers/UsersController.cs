using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Domain.Exceptions;
using SimpleBlog.Web.API.ViewModels;

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
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            await _userService.Register(request.Username, request.Password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            try
            {
                var token = await _userService.Login(request.Username, request.Password);
                return Ok(new { token });
            }
            catch (UserNotFoundException) { return Unauthorized("Credenciais inválidas"); }
            catch (InvalidPasswordException) { return Unauthorized("Credenciais inválidas"); }
        }
    }
}

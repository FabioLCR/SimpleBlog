using Microsoft.AspNetCore.Mvc;

namespace SimpleBlog.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

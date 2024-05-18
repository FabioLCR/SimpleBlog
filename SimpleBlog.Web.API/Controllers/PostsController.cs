using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Services;
using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : Controller
    {
        private readonly UserService _userService;
        private readonly PostService _postService;

        public PostsController(
            UserService userService,  
            PostService postService)
        {
            _userService = userService;
            _postService = postService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetAll();
            return Ok(posts);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _postService.GetById(id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PostEntity post)
        {
            var user = await _userService.GetLoggedInUser(User);
            if (user == null)
                return Unauthorized();

            post.UserId = user.Id;
            await _postService.Add(post);

            return Ok(post);
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PostEntity post)
        {
            var user = await _userService.GetLoggedInUser(User); 
            if (user == null)
                return Unauthorized();

            var existingPost = await _postService.GetById(id);
            if (existingPost == null)
                return NotFound();

            if (existingPost.UserId != user.Id)
                return Forbid();

            post.UserId = user.Id;
            await _postService.Update(post);

            return Ok(post);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetLoggedInUser(User);
            if (user == null)
                return Unauthorized();

            var existingPost = await _postService.GetById(id);
            if (existingPost == null)
                return NotFound();

            if (existingPost.UserId != user.Id)
                return Forbid();

            await _postService.Delete(id);

            return NoContent();
        }
    }
}
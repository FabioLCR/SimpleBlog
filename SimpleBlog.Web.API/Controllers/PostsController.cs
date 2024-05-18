using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Application.Services;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Exceptions;

namespace SimpleBlog.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public PostsController(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<PostEntity>> GetAll()
        {
            return await _postService.GetAll();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<PostEntity?> GetById(int id)
        {
            return await _postService.GetById(id);
        }

        [Authorize]
        [HttpPost]
        public async Task Create(PostEntity post)
        {
            var user = await _userService.GetLoggedInUser(User);
            await _postService.Add(post, user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task Update(int id, PostEntity post)
        {
            if (id != post.Id)
                throw new ArgumentException("ID na URL não corresponde ao ID do post");

            var user = await _userService.GetLoggedInUser(User);
            await _postService.Update(post, user);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var user = await _userService.GetLoggedInUser(User);
            await _postService.Delete(id, user);
        }
    }
}
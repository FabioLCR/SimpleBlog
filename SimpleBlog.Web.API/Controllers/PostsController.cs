using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Interfaces;
using SimpleBlog.Application.Mappers;
using SimpleBlog.Web.API.ViewModels;

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

        [HttpGet]
        public async Task<IEnumerable<PostResponse>> GetAll()
        {
            var posts = await _postService.GetAll();

            return posts.ToResponse();
        }

        [HttpGet("{id}")]
        public async Task<PostResponse?> GetById(int id)
        {
            var post = await _postService.GetById(id);

            return post?.ToResponse();
        }

        [Authorize]
        [HttpPost]
        public async Task Create(PostCreateRequest request)
        {
            var user = await _userService.GetLoggedInUser(User);

            await _postService.Add(request.ToDTO(user));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task Update(int id, PostUpdateRequest post)
        {
            if (id != post.Id)
                throw new ArgumentException("ID na URL não corresponde ao ID do post");

            var user = await _userService.GetLoggedInUser(User);
            await _postService.Update(post.ToDTO(user));
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
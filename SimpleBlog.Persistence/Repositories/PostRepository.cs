using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Persistence.Context;

namespace SimpleBlog.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SqliteContext _context;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(IDatabase database, ILogger<PostRepository> logger)
        {
            _context = (SqliteContext)database;
            _logger = logger;
        }

        public async Task<IEnumerable<PostEntity>> GetAll()
        {
            _logger.LogInformation("Obtendo todas as postagens");
            return await _context.Posts.Include(p => p.User).ToListAsync();
        }

        public async Task<PostEntity?> GetById(int id)
        {
            _logger.LogInformation("Obtendo postagem pelo ID: {Id}", id);
            return await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Add(PostEntity post)
        {
            _logger.LogInformation("Adicionando nova postagem com ID: {Id}", post.Id);
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task Update(PostEntity post)
        {
            _logger.LogInformation("Atualizando postagem com ID: {Id}", post.Id);
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Excluindo postagem com ID: {Id}", id);
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
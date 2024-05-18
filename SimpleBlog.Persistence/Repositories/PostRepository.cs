using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Persistence.Context;

namespace SimpleBlog.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SqliteContext _context;

        public PostRepository(IDatabase database)
        {
            _context = (SqliteContext)database;
        }

        public async Task<IEnumerable<PostEntity>> GetAll()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<PostEntity?> GetById(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task Add(PostEntity post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task Update(PostEntity post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
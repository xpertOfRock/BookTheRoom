using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }

        public async Task Update(int hotelId, string description)
        {
            await _context.Comments
                .Where(c => c.Id == hotelId)
                .ExecuteUpdateAsync(e => e
                .SetProperty(c => c.Description, description)
                .SetProperty(c => c.UpdatedAt, DateTime.UtcNow)
                );
        }
    }
}

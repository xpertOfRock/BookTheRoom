using Core.Entities;
using Core.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CommentRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task Add(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            
        }
        //public async Task<Comment> GetById(int? id)
        //{
        //    var comment = 

        //    return await _context.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        //}
        public async Task Delete(int id, string userId = "null")
        {
            if(userId == "null")
            {
                return;
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null )
            {
                return;
            }

            var comment = await _context.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return;
            }

            if (comment.UserId == user.Id || user.Role == UserRole.Admin) 
            {
                await _context.Comments.Where(c => c.Id == id).ExecuteDeleteAsync();
            }
        }

        public async Task Update(int id, string description)
        {
            await _context.Comments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(c => c.Description, description)
                .SetProperty(c => c.UpdatedAt, DateTime.UtcNow));
        }
    }
}

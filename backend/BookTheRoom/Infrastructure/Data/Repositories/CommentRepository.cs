using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Repositories
{
    public class CommentRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ICommentRepository
    {
        public async Task<IResult> Add(Comment comment, CancellationToken token = default)
        {
            await context.Comments.AddAsync(comment, token);
            return new Success("Entity 'Comment' was created successfully.");
        }

        //public async Task<Comment> GetById(int? id)
        //{
        //    var comment = 

        //    return await _context.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        //}

        public async Task<IResult> Delete(int id, string userId = "null", CancellationToken token = default)
        {
            if(userId == "null")
            {
                return new Fail("User is null.");
            }

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, token);

            if (user is null )
            {
                return new Fail("User is null."); ;
            }

            var comment = await context.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, token);

            if (comment is null)
            {
                throw new EntityNotFoundException<Comment>();
            }

            if (comment.UserId == user.Id || user.Role == UserRole.Admin) 
            {
                await context.Comments
                    .Where(c => c.Id == id)
                    .ExecuteDeleteAsync(token);
            }
            return new Success("Entity 'Comment' was deleted successfully.");
        }

        public async Task<IResult> Update(int id, string description, CancellationToken token = default)
        {
            await context.Comments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(c => c.Description, description)
                .SetProperty(c => c.UpdatedAt, DateTime.UtcNow),
                token);

            return new Success("Entity 'Comment' was updated successfully.");
        }
    }
}

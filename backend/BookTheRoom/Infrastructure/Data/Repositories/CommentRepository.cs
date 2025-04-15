using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repository for managing entities type of "Comment" in database using Entity Framework with cache aside pattern
    /// </summary>
    public class CommentRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ICommentRepository
    {
        public async Task<IResult> Add(Comment comment)
        {
            await context.Comments.AddAsync(comment);
            return new Success("Entity 'Comment' was created successfully.");
        }

        //public async Task<Comment> GetById(int? id)
        //{
        //    var comment = 

        //    return await _context.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        //}

        public async Task<IResult> Delete(int id, string userId = "null")
        {
            if(userId == "null")
            {
                return new Fail("User is null.");
            }

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null )
            {
                return new Fail("User is null."); ;
            }

            var comment = await context.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return new Fail("Impossible to delete a non-existent entity.");
            }

            if (comment.UserId == user.Id || user.Role == UserRole.Admin) 
            {
                await context.Comments
                    .Where(c => c.Id == id)
                    .ExecuteDeleteAsync();
            }
            return new Success("Entity 'Comment' was deleted successfully.");
        }

        public async Task<IResult> Update(int id, string description)
        {
            await context.Comments
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(c => c.Description, description)
                .SetProperty(c => c.UpdatedAt, DateTime.UtcNow));

            return new Success("Entity 'Comment' was updated successfully.");
        }
    }
}

using Core.Entities;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface which represents an abstraction for managing entities type of "Comment" in database using Redis and Entity Framework with cache aside pattern.
    /// </summary>
    public interface ICommentRepository
    {
        Task<IResult> Add(Comment comment);
        Task<IResult> Update(int id, string description);
        Task<IResult> Delete(int id, string userId = "null");
    }
}

using Core.Entities;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IResult> Add(Comment comment);
        Task<IResult> Update(int id, string description);
        Task<IResult> Delete(int id, string userId = "null");
    }
}

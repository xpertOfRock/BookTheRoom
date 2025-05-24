using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IResult> Add(Comment comment, CancellationToken token = default);
        Task<IResult> Update(int id, string description, CancellationToken token = default);
        Task<IResult> Delete(int id, string userId = "null", CancellationToken token = default);
        Task<List<Comment>> GetUserComments(string userId, GetUserCommentsRequest request, CancellationToken token = default);
    }
}

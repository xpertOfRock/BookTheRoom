using Core.Entities;
using Newtonsoft.Json.Linq;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IResult> Add(Comment comment, CancellationToken token = default);
        Task<IResult> Update(int id, string description, CancellationToken token = default);
        Task<IResult> Delete(int id, string userId = "null", CancellationToken token = default);
    }
}

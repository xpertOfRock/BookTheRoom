using Core.Entities;
using Core.Enums;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task<IResult> Add(Comment comment);
        Task<IResult> Update(int id, string description);
        Task<IResult> Delete(int id, string userId = "null");
        //Task<Comment> GetById(int? id);
    }
}

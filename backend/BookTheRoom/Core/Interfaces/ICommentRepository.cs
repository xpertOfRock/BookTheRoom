using Core.Entities;
using Core.Enums;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task Add(Comment comment);
        Task Update(int id, string description);
        Task Delete(int id, string userId = "null");
        Task<Comment> GetById(int id);
    }
}

using Core.Entities;

namespace Core.Interfaces
{
    public interface ICommentRepository
    {
        Task Add(Comment comment);
        Task Update(int hotelId, string description);
    }
}

using Core.Interfaces;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IHotelRepository Hotels { get; }
        IRoomRepository Rooms { get; }
        IOrderRepository Orders { get; }
        ICommentRepository Comments { get; }
        Task SaveChangesAsync();
    }
}

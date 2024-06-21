using BookTheRoom.Core.Interfaces;

namespace BookTheRoom.Infrastructure.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IHotelRepository Hotels { get; }
        IRoomRepository Rooms { get; }
        IOrderRepository Orders { get; }
        IAddressRepository Addresses { get; }
        Task SaveChangesAsync();
    }
}

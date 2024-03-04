using BookTheRoom.Domain.Entities;

namespace BookTheRoom.Application.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<Room> GetRoomByIdAsync(int id);
        Task<IEnumerable<Room>> GetAllRoomsByHotel(Hotel hotel);      
    }
}

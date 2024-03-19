using BookTheRoom.Domain.Entities;

namespace BookTheRoom.Application.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<Room> GetRoomByIdAsync(int hotelId, int id);
        Task<List<Room>> GetAllRoomsByHotel(int id);      
    }
}

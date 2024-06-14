using BookTheRoom.Domain.Entities;

namespace BookTheRoom.Application.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<Room> GetRoomByNumber(int hotelId, int number);
        Task<List<Room>> GetAllRoomsByHotel(int id);      
    }
}

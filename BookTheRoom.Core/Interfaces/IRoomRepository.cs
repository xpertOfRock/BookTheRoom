using BookTheRoom.Core.Entities;

namespace BookTheRoom.Core.Interfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<Room> GetByNumber(int hotelId, int number);
        Task<List<Room>> GetAllRoomsByHotel(int id);      
    }
}

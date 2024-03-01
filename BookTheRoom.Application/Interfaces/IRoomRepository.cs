using BookTheRoom.Domain.Entities;

namespace BookTheRoom.Application.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> GetRoomByNumberAsync(int number);
        Task<List<Room>> GetAllRoomsByHotel(Hotel hotel);
        Task Add(Room room);
        Task Delete(Room room);
        Task Update(Room room);
    }
}

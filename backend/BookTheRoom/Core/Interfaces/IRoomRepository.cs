using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAll(int hotelId, GetRoomsRequest request);
        Task<List<Room>> GetAllRooms();
        Task<Room> GetById(int? hotelId, int? number);
        Task Add(Room room);
        Task Update(int hotelId, int number, UpdateRoomRequest request);
        Task Delete(int hotelId, int number);       
    }
}

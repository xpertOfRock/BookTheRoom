using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAll(int hotelId, GetRoomsRequest request);
        Task<List<Room>> GetAllRooms();
        Task<Room?> GetById(int? hotelId, int? number, CancellationToken cancellationToken = default);
        Task<IResult> Add(Room room);
        Task<IResult> Update(int hotelId, int number, UpdateRoomRequest request);
        Task<IResult> Delete(int hotelId, int number);       
    }
}

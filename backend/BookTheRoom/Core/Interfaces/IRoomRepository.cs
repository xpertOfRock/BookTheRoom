using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAll(int hotelId, GetRoomsRequest request, CancellationToken token = default);
        Task<List<Room>> GetAllRooms(CancellationToken token = default);
        Task<Room?> GetById(int hotelId, int number, CancellationToken cancellationToken = default);
        Task<IResult> Add(Room room, CancellationToken token = default);
        Task<IResult> Update(int hotelId, int number, UpdateRoomRequest request, CancellationToken token = default);
        Task<IResult> Delete(int hotelId, int number, CancellationToken token = default);       
    }
}

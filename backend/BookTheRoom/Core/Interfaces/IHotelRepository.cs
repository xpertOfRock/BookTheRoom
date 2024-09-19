using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAll(GetDataRequest request);
        Task<List<Comment>> GetAllComments(int hotelId);
        Task<Hotel> GetById(int id);
        Task Add(Hotel hotel);
        Task Update(int id, UpdateHotelRequest request);
        Task Delete(int id);
    }
}

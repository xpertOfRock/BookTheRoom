using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAll(GetHotelsRequest request);
        Task<Hotel> GetById(int? id);
        Task Add(Hotel hotel);
        Task Update(int id, UpdateHotelRequest request);
        Task Delete(int id);
    }
}

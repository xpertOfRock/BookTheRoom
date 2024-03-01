using BookTheRoom.Domain.Entities;

namespace BookTheRoom.Application.Interfaces
{
    public interface IHotelRepository
    {
        Task<Hotel> GetHotelByIdAsync(int id);
        Task<List<Hotel>> GetAllHotels();
        Task<Hotel> GetHotelByAddress(Address address);
        Task<List<Hotel>> GetAllHotelsByAddress(Address address);
        Task Add(Hotel hotel);
        Task Delete(Hotel hotel);
        Task Update(Hotel hotel);
    }
}

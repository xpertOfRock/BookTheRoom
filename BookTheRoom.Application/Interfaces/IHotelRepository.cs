using BookTheRoom.Domain.Entities;

namespace BookTheRoom.Application.Interfaces
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<Hotel> GetByIdGetByIdInclude(int id);
        Task<IEnumerable<Hotel>> GetAllInclude();
        Task<Hotel> GetHotelByAddress(string country, string city, string street, int building);
        Task<IEnumerable<Hotel>> GetAllHotelsByAddress(string country, string? city, string? street, int? building);
    }
}

using BookTheRoom.Domain.Entities;

namespace BookTheRoom.Application.Interfaces
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<List<Hotel>> GetAllHotelsByAddress(string? country, string? city, string? streetOrDistrict, int? index);
    }
}

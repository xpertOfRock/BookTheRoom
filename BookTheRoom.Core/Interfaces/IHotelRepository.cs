using BookTheRoom.Core.Entities;

namespace BookTheRoom.Core.Interfaces
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<List<Hotel>> GetAllHotelsByAddress(string? country, string? city, string? streetOrDistrict, int? index);
    }
}

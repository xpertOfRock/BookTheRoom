using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAll(GetHotelsRequest request, CancellationToken token = default);
        Task<Hotel?> GetById(int id, CancellationToken token = default);
        Task<IResult> Add(Hotel hotel, CancellationToken token = default);
        Task<IResult> Update(int id, UpdateHotelRequest request, CancellationToken token = default);
        Task<IResult> Delete(int id, CancellationToken token = default);
        Task UpdateCache(Hotel hotel, CancellationToken cancellationToken = default);
    }
}

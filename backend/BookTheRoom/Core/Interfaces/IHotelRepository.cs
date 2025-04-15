using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface which represents an abstraction for managing entities type of "Hotel" in database using Redis and Entity Framework with cache aside pattern.
    /// </summary>
    public interface IHotelRepository
    {
        Task<List<Hotel>> GetAll(GetHotelsRequest request);
        Task<Hotel?> GetById(int id, CancellationToken cancellationToken = default);
        Task<IResult> Add(Hotel hotel);
        Task<IResult> Update(int id, UpdateHotelRequest request);
        Task<IResult> Delete(int id);
        Task UpdateCache(Hotel hotel, CancellationToken cancellationToken = default);
    }
}

using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IApartmentRepository
    {
        Task<List<Apartment>> GetAll(GetApartmentsRequest request);
        Task<List<Apartment>> GetAllUsersApartments(string userId, GetApartmentsRequest request);
        Task<Apartment?> GetById(int? id, CancellationToken cancellationToken = default);
        Task<IResult> Add(Apartment apartment);
        Task<IResult> Update(int id, string userId, UpdateApartmentRequest request);
        Task<IResult> Delete(int id, string userId);
        Task UpdateCache(Apartment apartment, CancellationToken cancellationToken = default);
    }
}

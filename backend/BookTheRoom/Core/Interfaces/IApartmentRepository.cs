using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IApartmentRepository
    {
        Task<List<Apartment>> GetAll(GetApartmentsRequest request, CancellationToken token = default);
        Task<List<Apartment>> GetAllUsersApartments(string userId, GetApartmentsRequest request, CancellationToken token = default);
        Task<Apartment?> GetById(int id, CancellationToken token = default);
        Task<IResult> Add(Apartment apartment, CancellationToken token = default);
        Task<IResult> Update(int id, string userId, UpdateApartmentRequest request, CancellationToken token = default);
        Task<IResult> Delete(int id, string userId, CancellationToken token = default);
        Task UpdateCache(Apartment apartment, CancellationToken token = default);
    }
}

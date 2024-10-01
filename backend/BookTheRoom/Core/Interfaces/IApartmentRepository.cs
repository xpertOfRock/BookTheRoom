using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IApartmentRepository
    {
        Task<List<Apartment>> GetAll(GetApartmentsRequest request);
        Task<List<Apartment>> GetAllUsersApartments(string userId, GetApartmentsRequest request);
        Task<Apartment> GetById(int? id);
        Task Add(Apartment apartment);
        Task Update(int id, UpdateApartmentRequest request);
        Task Delete(int id);
    }
}

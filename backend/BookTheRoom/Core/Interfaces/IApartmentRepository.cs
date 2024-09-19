using Core.Contracts;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IApartmentRepository
    {
        Task<List<Apartment>> GetAll(GetDataRequest request);
        Task<Apartment> GetById(int id);
        Task Add(Apartment apartment);
        Task Update(int id, UpdateApartmentRequest request);
        Task Delete(int id);
    }
}

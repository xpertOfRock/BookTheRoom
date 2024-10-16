using Core.Entities;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetAll(int hotelId);
        Task<Service> GetService(int hotelId, OrderOptionsName optionsName);
        Task Add(Service service);
        Task Update(int hotelId, OrderOptionsName optionName, decimal price);
    }
}

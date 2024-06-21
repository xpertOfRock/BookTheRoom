using BookTheRoom.Core.ValueObjects;

namespace BookTheRoom.Core.Interfaces
{
    public interface IAddressRepository 
    {
        Task<List<Address>> GetAllAsync();
        Task<Address> GetAddress(string country, string city, string streetOrDistrict, int index);
        Task Add(Address address);
        void Delete(Address address);
        void Update(Address address);
    }
}

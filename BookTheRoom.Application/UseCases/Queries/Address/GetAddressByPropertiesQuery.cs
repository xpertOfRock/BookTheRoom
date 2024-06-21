using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Address
{
    public class GetAddressByPropertiesQuery : IRequest<Core.ValueObjects.Address>
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetOrDistrict { get; set; }
        public int Index { get; set; }
        public GetAddressByPropertiesQuery(string country, string city, string streetOrDistrict, int index)
        {
            Country = country;
            City = city;
            StreetOrDistrict = streetOrDistrict;
            Index = index;
        }
    }
}

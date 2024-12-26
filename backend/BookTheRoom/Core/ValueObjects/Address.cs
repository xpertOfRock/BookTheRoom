using System.Text.Json;

namespace Core.ValueObjects
{
    public class Address
    {
        public string Country { get; }
        public string State { get; }
        public string City { get; }      
        public string Street { get; }            
        public string PostalCode { get; }


        public Address(string country, string state, string city, string street, string postalCode)
        {
            Country = country;          
            State = state;
            City = city;
            Street = street;           
            PostalCode = postalCode;          
        }

        public string ToString(bool shortForm = false)
        {
            var result = shortForm == true ? $"{Country}, {City}" : $"{Country}, {State}, {City}, {Street}, {PostalCode}";
            return result;
        }
        public static string AsJson(Address address) 
        {
            var result = JsonSerializer.Serialize(address);
            return result;
        }
    }
}

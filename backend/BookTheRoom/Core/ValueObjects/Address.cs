namespace Core.ValueObjects
{
    public class Address
    {
        public string Country { get; }
        public string City { get; }
        public string State { get; }
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

        public override string ToString()
        {
            return $"{Country}, {State}, {City}, {Street}, {PostalCode}";
        }
    }
}

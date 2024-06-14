namespace BookTheRoom.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetOrDistrict { get; set; }
        public int Index { get; set; }
    }
}

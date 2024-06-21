namespace BookTheRoom.Core.ValueObjects
{
    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string StreetOrDistrict { get; set; }
        public int Index { get; set; }
    }
}

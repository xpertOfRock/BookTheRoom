namespace BookTheRoom.Domain.Entities
{
    public class HotelImage
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public int HotelId{ get; set; }

        public Hotel Hotel{ get; set; }
    }
}

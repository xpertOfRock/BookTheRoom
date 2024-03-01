namespace BookTheRoom.Domain.Entities
{
    public class Order
    {     
        public int Id { get; set; }
        public decimal OverallPrice { get; set; }        
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public Room Room { get; set; }
        public Hotel Hotel { get; set; }                   
    }
}

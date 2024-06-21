using BookTheRoom.Core.Enums;

namespace BookTheRoom.Core.Entities
{
    public class Order
    {     
        public int Id { get; set; }
        public decimal OverallPrice { get; set; }                
        public DateTime CheckIn { get; set; } = DateTime.UtcNow;
        public DateTime CheckOut { get; set; } = DateTime.UtcNow.AddDays(1);
        public string? AnonymousEmail { get; set; } = null;
        public string? AnonymousNumber { get; set; } = null;
        public string? UserId { get; set; } = null;
        public int RoomId { get; set; }
        public int HotelId { get; set; }

        public OrderStatus Status { get; set; }

    }
}

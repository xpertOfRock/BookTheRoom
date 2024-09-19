using Core.Enums;

namespace Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal OverallPrice { get; set; }        
        public string? Email { get; set; } = null;
        public string? Phone { get; set; } = null;
        public bool MinibarIncluded { get; set; } = false;
        public bool MealsIncluded { get; set; } = false;
        public string? UserId { get; set; } = null;
        public int RoomNumber { get; set; }
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime CreatedAt { get; set; }


        public Hotel Hotel { get; set; }
        public Room Room { get; set; }
        public OrderStatus Status { get; set; }      
    }
}

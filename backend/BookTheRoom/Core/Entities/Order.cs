using Core.Enums;

namespace Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public decimal OverallPrice { get; set; }        
        public string? Email { get; set; } = null;
        public string? Phone { get; set; } = null;
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public bool MinibarIncluded { get; set; } = false;
        public bool MealsIncluded { get; set; } = false;
        public string? UserId { get; set; } = null;
        public int RoomNumber { get; set; }
        public int HotelId { get; set; }
        public DateTimeOffset CheckIn { get; set; }
        public DateTimeOffset CheckOut { get; set; }
        public DateTimeOffset CreatedAt { get; set; }


        public OrderStatus Status { get; set; }      
    }
}

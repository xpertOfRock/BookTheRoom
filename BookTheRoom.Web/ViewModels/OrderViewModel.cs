using BookTheRoom.Domain.Entities;
using BookTheRoom.Domain.Enums;

namespace BookTheRoom.WebUI.ViewModels
{
    public class OrderViewModel : Order
    {
        public Room? Room { get; set; }
        public Hotel? Hotel { get; set; }
    }
}

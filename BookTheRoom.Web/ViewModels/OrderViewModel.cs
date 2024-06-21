using BookTheRoom.Core.Entities;

namespace BookTheRoom.Web.ViewModels
{
    public class OrderViewModel : Order
    {
        public Room? Room { get; set; }
        public Hotel? Hotel { get; set; }
    }
}

using BookTheRoom.Core.Entities;

namespace BookTheRoom.Web.ViewModels
{
    public class EditRoomViewModel : Room
    {
        public IFormFile? PreviewImage { get; set; }
        public List<IFormFile>? RoomImages { get; set; }
    }
}

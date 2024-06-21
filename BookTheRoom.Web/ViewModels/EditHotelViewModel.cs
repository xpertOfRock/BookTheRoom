using BookTheRoom.Core.Entities;

namespace BookTheRoom.Web.ViewModels
{
    public class EditHotelViewModel : Hotel
    {
        public IFormFile? PreviewImage { get; set; }
        public List<IFormFile>? HotelImages { get; set; }
    }
}

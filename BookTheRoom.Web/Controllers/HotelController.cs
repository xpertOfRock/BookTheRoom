using BookTheRoom.Application.Interfaces;
using BookTheRoom.Domain.Entities;
using BookTheRoom.Infrastructure.Identity;
using BookTheRoom.Infrastructure.Data.Interfaces;
using BookTheRoom.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

namespace BookTheRoom.WebUI.Controllers
{
    public class HotelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        public HotelController(IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        [Route("Hotels", Name = "Hotels")]
        public async Task<IActionResult> Hotels()
        {
            var hotels = await _unitOfWork.Hotels.GetAll();
            return View(hotels);
        }

        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        [Route("Hotels/{id:int}", Name = "Hotel")]
        public async Task<IActionResult> Hotel([FromRoute] int id)
        {
            var hotel = await _unitOfWork.Hotels.GetById(id);
            if (hotel == null)
            {
                return Redirect($"/Hotels/{id}");
            }
            return View(hotel);
        }

        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        public IActionResult AddHotel()
        {
            return View();
        }


        [HttpPost]
        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/Add", Name = "AddHotel")]
        public async Task<IActionResult> AddHotel(AddHotelViewModel hotel)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(hotel.PreviewImage.Name,
                                                               hotel.PreviewImage.OpenReadStream());

                hotel.PreviewImage.OpenReadStream().Dispose();

                var hotelImages = new List<string>();

                foreach (var item in hotel.HotelImages)
                {
                    var resultForList = await _photoService.AddPhotoAsync(item.Name, item.OpenReadStream());
                    hotelImages.Add(resultForList.Url.ToString());
                    item.OpenReadStream().Dispose();
                }

                hotel.ImagesURL = hotelImages;
                hotel.PreviewURL = result.Url.ToString();

                await _unitOfWork.Hotels.Update(hotel);

                await _unitOfWork.SaveChangesAsync();

                return Redirect($"/Hotels");
            }
            return View(hotel);
        }


        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("/Hotels/{id:int}/Edit", Name = "EditHotel")]
        public async Task<IActionResult> EditHotel([FromRoute] int id)
        {
            var thisHotel = await _unitOfWork.Hotels.GetById(id);
            var editViewModel = new EditHotelViewModel
            {
                Id = id,
                Name = thisHotel.Name,
                Description = thisHotel.Description,
                AddressId = thisHotel.AddressId,
                Address = thisHotel.Address,
                HasPool = thisHotel.HasPool,
                NumberOfRooms = thisHotel.NumberOfRooms,
                Rating = thisHotel.Rating
            };
            return View(editViewModel);
        }


        [HttpPost]
        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("/Hotels/{id:int}/Edit", Name = "EditHotel")]
        public async Task<IActionResult> EditHotel([FromRoute] int id, EditHotelViewModel hotel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit hotel.");
                return View("EditHotel", hotel);
            }

            var thisHotel = await _unitOfWork.Hotels.GetById(id);
            var thisAddress = await _unitOfWork.Addresses.GetById(thisHotel.AddressId);
            var previewImage = thisHotel.PreviewURL;
            var hotelImages = thisHotel.ImagesURL;

            if (thisHotel != null)
            {
                if (hotel.PreviewImage != null)
                {
                    await _photoService.DeletePhotoAsync(thisHotel.PreviewURL);

                    var result = await _photoService.AddPhotoAsync(hotel.PreviewImage.Name,
                                                           hotel.PreviewImage.OpenReadStream());

                    hotel.PreviewImage.OpenReadStream().Dispose();
                    previewImage = result.Url.ToString();

                }

                if (!hotel.HotelImages.IsNullOrEmpty())
                {

                    foreach (var image in thisHotel.ImagesURL)
                    {

                        await _photoService.DeletePhotoAsync(image);
                    }

                    hotelImages.Clear();

                    foreach (var image in hotel.HotelImages)
                    {
                        var resultForList = await _photoService.AddPhotoAsync(image.Name, image.OpenReadStream());
                        image.OpenReadStream().Dispose();
                        hotelImages.Add(resultForList.Url.ToString());

                    }
                }
            }
            
            if (thisAddress.Country != hotel.Address.Country ||
                thisAddress.City != hotel.Address.City ||
                thisAddress.StreetOrDistrict != hotel.Address.StreetOrDistrict ||
                thisAddress.Index != hotel.Address.Index)
            {
                thisAddress.Country = hotel.Address.Country;
                thisAddress.City = hotel.Address.City;
                thisAddress.StreetOrDistrict = hotel.Address.StreetOrDistrict;
                thisAddress.Index = hotel.Address.Index;
                await _unitOfWork.Addresses.Update(thisAddress);
            }

            hotel.ImagesURL = hotelImages;
            hotel.PreviewURL = previewImage;
            hotel.Address = thisAddress;

            await _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.SaveChangesAsync();

            return Redirect($"/Hotels/{id}");
        }


        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{id:int}/Delete", Name = "DeleteHotel")]
        public async Task<IActionResult> DeleteHotel([FromRoute] int id)
        {
            var hotelToDelete = await _unitOfWork.Hotels.GetById(id);
            if (hotelToDelete == null) { return View("Error"); }

            return View(hotelToDelete);
        }


        [HttpPost, ActionName("DeleteHotel")]
        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{id:int}/Delete", Name = "DeleteHotel")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var hotelToDelete = await _unitOfWork.Hotels.GetById(id);
            if (hotelToDelete != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(hotelToDelete.PreviewURL);

                    foreach (var image in hotelToDelete.ImagesURL)
                    {
                        await _photoService.DeletePhotoAsync(image);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Couldn't delete the photo.");
                    return View(hotelToDelete);
                }
            }
            if (hotelToDelete == null) { return View("Error"); }

            _unitOfWork.Addresses.Delete(hotelToDelete.Address);
            _unitOfWork.Hotels.Delete(hotelToDelete);
            await _unitOfWork.SaveChangesAsync();
            return Redirect("/Hotels");
        }       
    }
}

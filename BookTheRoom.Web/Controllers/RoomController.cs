using BookTheRoom.Application.Interfaces;
using BookTheRoom.Core.Entities;
using BookTheRoom.Infrastructure.Identity;
using BookTheRoom.Infrastructure.Data.Interfaces;
using BookTheRoom.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using BookTheRoom.Application.UseCases.Commands.Hotel;

namespace BookTheRoom.WebUI.Controllers
{

    public class RoomController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        public RoomController(IMediator mediator, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }
        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        [Route("Hotels/{hotelId:int}/Rooms", Name = "Rooms")]
        public async Task<IActionResult> Rooms([FromRoute] int hotelId)
        {
            var rooms = await _unitOfWork.Rooms.GetAllRoomsByHotel(hotelId);
            ViewBag.HotelId = hotelId;
            return View(rooms);
        }

        [AllowAnonymous]
        [EnableRateLimiting("fixed")]
        [Route("Hotels/{hotelId:int}/Rooms/{number:int}", Name = "Room")]
        public async Task<IActionResult> Room([FromRoute]int hotelId, [FromRoute]int number)
        {
            var room = await _unitOfWork.Rooms.GetByNumber(hotelId, number);
            if (room == null)
            {
                return Redirect($"Hotels/{hotelId}");
            }
            return View(room);
        }

        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{hotelId:int}/Rooms/Add", Name = "AddRoom")]
        public IActionResult AddRoom([FromRoute] int hotelId)
        {
            var roomViewModel = new AddRoomViewModel
            {
                HotelId = hotelId
            };
            return View(roomViewModel);
        }

        //Perfect variant for 2 people. Room has 1 bed, 1 sofa-bed and great view on the mountains!
        [HttpPost]        
        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{hotelId:int}/Rooms/Add", Name = "AddRoom")]
        public async Task<IActionResult> AddRoom([FromRoute] int hotelId, AddRoomViewModel room)
        {                      
            if (ModelState.IsValid)
            {
                var thisHotel = await _unitOfWork.Hotels.GetById(hotelId);
                var result = await _photoService.AddPhotoAsync(room.PreviewImage.Name,
                                                               room.PreviewImage.OpenReadStream());
                room.PreviewImage.OpenReadStream().Dispose();

                var roomImages = new List<string>();

                foreach (var item in room.RoomImages)
                {
                    var resultForList = await _photoService.AddPhotoAsync(item.Name, item.OpenReadStream());
                    roomImages.Add(resultForList.Url.ToString());
                    item.OpenReadStream().Dispose();
                }

                room.IsFree = true;
                room.HotelId = hotelId;
                room.PreviewURL = result.Url.ToString();
                room.ImagesURL = roomImages;

                
                await _unitOfWork.Rooms.Add(room);

                thisHotel.Rooms.Add(room);

                await _mediator.Send(new UpdateHotelCommand(thisHotel));

                await _unitOfWork.SaveChangesAsync();

                return Redirect($"/Hotels/{hotelId}/Rooms");
            }
            return View(room);
        }


        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{hotelId:int}/Rooms/{number:int}/Edit", Name = "EditRoom")]
        public async Task<IActionResult> EditRoom([FromRoute] int hotelId, [FromRoute] int number)
        {
            var thisHotel = await _unitOfWork.Hotels.GetById(hotelId);
            var thisRoom = await _unitOfWork.Rooms.GetByNumber(hotelId, number);
            var editViewModel = new EditRoomViewModel
            {
                Number = number,
                Description = thisRoom.Description,                
                PriceForRoom = thisRoom.PriceForRoom,
                RoomCategory = thisRoom.RoomCategory,
                HotelId = thisHotel.Id,
            };
            return View(editViewModel);
        }


        [HttpPost]
        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{hotelId:int}/Rooms/{number:int}/Edit", Name = "EditRoom")]
        public async Task<IActionResult> EditRoom([FromRoute] int hotelId, [FromRoute] int number, EditRoomViewModel room)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit room.");
                return View("EditRoom", room);
            }

            var thisHotel = await _unitOfWork.Hotels.GetById(hotelId);
            var thisRoom = await _unitOfWork.Rooms.GetByNumber(hotelId, number);

            var previewImage = thisRoom.PreviewURL;
            var roomImages = thisRoom.ImagesURL;
            var roomCategory = thisRoom.RoomCategory;
            if (thisHotel != null)
            {
                if (room.PreviewImage != null)
                {
                    await _photoService.DeletePhotoAsync(thisRoom.PreviewURL);

                    var result = await _photoService.AddPhotoAsync(room.PreviewImage.Name,
                                                           room.PreviewImage.OpenReadStream());

                    room.PreviewImage.OpenReadStream().Dispose();
                    previewImage = result.Url.ToString();

                }

                if (!room.RoomImages.IsNullOrEmpty())
                {
                    foreach (var image in thisRoom.ImagesURL)
                    {
                        await _photoService.DeletePhotoAsync(image);
                    }

                    roomImages.Clear();

                    foreach (var image in room.RoomImages)
                    {
                        var resultForList = await _photoService.AddPhotoAsync(image.Name, image.OpenReadStream());                        
                        image.OpenReadStream().Dispose();
                        roomImages.Add(resultForList.Url.ToString());
                    }
                }
            }



            thisRoom.Id = thisRoom.Id;
            thisRoom.Number = number;
            thisRoom.Description = room.Description;
            thisRoom.PriceForRoom = room.PriceForRoom;
            thisRoom.IsFree = thisRoom.IsFree;
            thisRoom.PreviewURL = previewImage;
            thisRoom.ImagesURL = roomImages;
            thisRoom.HotelId = hotelId;
            thisRoom.RoomCategory = room.RoomCategory;

            _unitOfWork.Rooms.Update(thisRoom);
            await _unitOfWork.SaveChangesAsync();

            return Redirect($"/Hotels/{hotelId}/Rooms/{number}");
        }


        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{hotelId:int}/Rooms/{number:int}/Delete", Name = "DeleteRoom")]
        public async Task<IActionResult> DeleteRoom([FromRoute] int hotelId, [FromRoute] int number)
        {
            var thisHotel = await _unitOfWork.Hotels.GetById(hotelId);
            var thisRoom = await _unitOfWork.Rooms.GetByNumber(thisHotel.Id, number);
            if (thisHotel == null) { return View("Error"); }

            return View(thisRoom);
        }


        [HttpPost, ActionName("DeleteRoom")]
        [EnableRateLimiting("fixed")]
        [Authorize(Roles = UserRole.Admin)]
        [Route("Hotels/{hotelId:int}/Rooms/{number:int}/Delete", Name = "DeleteRoom")]
        public async Task<IActionResult> Delete([FromRoute] int hotelId, [FromRoute] int number)
        {
            var thisHotel = await _unitOfWork.Hotels.GetById(hotelId);
            var thisRoom = await _unitOfWork.Rooms.GetByNumber(thisHotel.Id, number);
            if (thisHotel == null && thisRoom == null) { return View("Error"); }

            if (thisRoom != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(thisRoom.PreviewURL);

                    foreach (var image in thisRoom.ImagesURL)
                    {
                        await _photoService.DeletePhotoAsync(image);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Couldn't delete the photo.");
                    return View(thisRoom);
                }
            }

            thisHotel.Rooms.Remove(thisRoom);

            await _mediator.Send(new UpdateHotelCommand(thisHotel));
            _unitOfWork.Rooms.Delete(thisRoom);

            await _unitOfWork.SaveChangesAsync();
            return Redirect($"/Hotels/{hotelId}/Rooms");
        }
    }
}

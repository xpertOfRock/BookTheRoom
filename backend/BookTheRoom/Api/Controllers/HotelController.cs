using Api.Contracts.Hotel;
using Api.DTOs;
using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Queries.Hotel;
using Core.Contracts;
using Core.Entities;
using Core.ValueObjects;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPhotoService _photoService;
        public HotelController(IMediator mediator, IPhotoService photoService)
        {
            _mediator = mediator;
            _photoService = photoService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] GetHotelsRequest request)
        {
            var hotels = await _mediator.Send(new GetHotelsQuery(request));

            var hotelsDTO = hotels.Select(h => new HotelsDTO(
                h.Id,
                h.Name,
                h.Images != null &&
                    h.Images.Any() 
                    ? h.Images.First() 
                    : "No Image",
                h.Rating,
                h.Address.ToString()
                )
            ).ToList();

            return Ok(new GetHotelsResponse(hotelsDTO));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var hotel = await _mediator.Send(new GetHotelQuery(id));

            if(hotel is null)
            {
                return NotFound($"Hotel with ID: {id} doesn't exist.");
            }

            var hotelDTO = new HotelDTO(
                hotel.Id,
                hotel.Name,
                hotel.Description,
                hotel.Address.ToString(),
                hotel.Rating,

                hotel.Images != null &&
                    hotel.Images.Any() 
                    ? hotel.Images 
                    : new List<string> {""},

                hotel.Rooms != null &&
                    hotel.Rooms.Any()
                    ? hotel.Rooms
                    : new List<Room>{ },
               
                hotel.Comments != null &&
                    hotel.Comments.Any()
                    ? hotel.Comments
                    : new List<Comment> { }
                );

            return Ok(hotelDTO);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Post([FromForm] CreateHotelForm form)
        {
            var imagesUrl = new List<string>();

            if (form.Images is not null && form.Images.Any())
            {
                foreach (var file in form.Images)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                        imagesUrl.Add(resultForList.Url.ToString());
                    }
                }
            }            
            
            var request = new CreateHotelRequest
            (
                form.Name,
                form.Description,
                form.Rating,
                form.Pool,
                new Address
                (
                    form.Country,
                    form.State,
                    form.City,
                    form.Street,
                    form.PostalCode
                ),
                imagesUrl
            );

           await _mediator.Send(new CreateHotelCommand(request));
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Put(int id, [FromForm] UpdateHotelForm form)
        {
            var images = new List<string>();

            if (form.Images is not null && form.Images.Any())
            {               
                foreach (var file in form.Images)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                        images.Add(resultForList.Url.ToString());
                    }
                }
            }

            var request = new UpdateHotelRequest
            (
                form.Name,
                form.Description,
                form.Rating,
                form.Pool,

                images.Any() 
                    ? images 
                    : null,

                null
            );            

            await _mediator.Send(new UpdateHotelCommand(id, request));
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteHotelCommand(id));
            return Ok();
        }
    }
}

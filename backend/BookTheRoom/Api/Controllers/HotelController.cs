using Api.Contracts;
using Api.DTOs;
using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Queries.Hotel;
using Core.Contracts;
using Core.Entities;
using MediatR;
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
                h.Address
                )
            ).ToList();

            return Ok(new GetHotelsResponse(hotelsDTO));
        }

        [HttpGet("{id}")]
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

                hotel.Images != null &&
                    hotel.Images.Any() 
                    ? hotel.Images 
                    : new List<string> {""},

                hotel.Rooms != null &&
                    hotel.Rooms.Any()
                    ? hotel.Rooms
                    : new List<Room> { },

                hotel.Address
                );

            return Ok(hotelDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateHotelRequest request)
        {
            await _mediator.Send(new CreateHotelCommand(request));
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] UpdateHotelRequest request,
            [FromForm] List<IFormFile>? files
            )
        {
            if(files.Any())
            {
                foreach (var file in files)
                {
                    var resultForList = await _photoService.AddPhotoAsync(file.Name, file.OpenReadStream());
                    request.Images.Add(resultForList.Url.ToString());
                    file.OpenReadStream().Dispose();
                }
            }
                        
            await _mediator.Send(new UpdateHotelCommand(id, request));
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteHotelCommand(id));
            return Ok();
        }
    }
}

using Api.Contracts;
using Api.DTOs;
using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Queries.Hotel;
using CloudinaryDotNet.Actions;
using Core.Contracts;
using Core.Entities;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


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
        public async Task<IActionResult> GetAll([FromQuery] GetDataRequest request)
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

                hotel.Images != null &&
                    hotel.Images.Any() 
                    ? hotel.Images 
                    : new List<string> {""},

                hotel.Rooms != null &&
                    hotel.Rooms.Any()
                    ? hotel.Rooms
                    : new List<Room> { }
               
                );

            return Ok(hotelDTO);
        }

        [HttpPost]
        //[Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Post([FromBody] CreateHotelRequest request, [FromForm] List<IFormFile>? files)
        {
            if (files.Any())
            {
                foreach (var file in files)
                {
                    var resultForList = await _photoService.AddPhotoAsync(file.Name, file.OpenReadStream());
                    request.Images.Add(resultForList.Url.ToString());
                    file.OpenReadStream().Dispose();
                }
            }
            await _mediator.Send(new CreateHotelCommand(request));
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin)]
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
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteHotelCommand(id));
            return Ok();
        }
    }
}

using Api.Contracts.Apartment;
using Application.UseCases.Commands.Apartment;
using Application.UseCases.Queries.Apartment;
using Core.Abstractions;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApartmentController(
            ISender sender,
            IPhotoService photoService,
            IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _sender = sender;
            _photoService = photoService;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("user-apartments")]
        [AllowAnonymous]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> GetAllUsersApartments([FromQuery] GetApartmentsRequest request)
        {
            var userId = _contextAccessor.HttpContext!.User.GetUserId() ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);

            var apartments = await _sender.Send(new GetUsersApartmentsQuery(userId, request));

            var apartmentsDTO = apartments.Select(a => new ApartmentsDTO(
                a.Id,
                a.Title,
                a.PriceForNight,
                a.Address.ToString(),
                a.CreatedAt,

                a.Images != null &&
                    a.Images.Any()
                    ? a.Images.First()
                    : "No Image",

                a.Comments != null && a.Comments.Any()
                    ? a.Comments
                            .Where(c => c.UserScore != null && c.UserScore > 0)
                            .Average(c => c.UserScore)
                    : -1f
                )       
            ).ToList();

            return Ok(new GetApartmentsResponse(apartmentsDTO));
        }
        [HttpGet]
        [AllowAnonymous]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> GetAll([FromQuery] GetApartmentsRequest request)
        {
            var apartments = await _sender.Send(new GetApartmentsQuery(request));

            var apartmentsDTO = apartments.Select(a => new ApartmentsDTO(
                a.Id,
                a.Title,
                a.PriceForNight,
                a.Address.ToString(true),
                a.CreatedAt,

                a.Images != null &&
                    a.Images.Any()
                    ? a.Images.First()
                    : "No Image",

                a.Comments != null && a.Comments.Any()
                    ? a.Comments
                            .Where(c => c.UserScore != null && c.UserScore > 0)
                            .Average(c => c.UserScore)
                    : -1f
                )                           
            ).ToList();
            
            return Ok(new GetApartmentsResponse(apartmentsDTO));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> Get(int id)
        {
            var apartment = await _sender.Send(new GetApartmentQuery(id));

            if (apartment is null)
            {
                return NotFound($"Hotel with ID: {id} doesn't exist.");
            }

            var apartmentDTO = new ApartmentDTO(
                apartment.Id,
                apartment.Title,
                apartment.Description,
                apartment.OwnerName,
                apartment.Email,
                apartment.PhoneNumber,
                apartment.PriceForNight,
                apartment.Telegram ?? string.Empty,
                apartment.Instagram ?? string.Empty,
                apartment.Address.ToString(),

                apartment.Comments != null && apartment.Comments.Any()
                        ? apartment.Comments
                            .Where(c => c.UserScore != null && c.UserScore > 0)
                            .Average(c => c.UserScore)
                        : -1f,

                apartment.CreatedAt,

                apartment.Images is not null &&
                    apartment.Images.Any()
                    ? apartment.Images
                    : new List<string> { "" },

                apartment.Comments is not null &&
                    apartment.Comments.Any()
                    ? apartment.Comments
                    : new List<Comment> { },


                apartment.Chats is not null &&
                    apartment.Chats.Any()
                    ? apartment.Chats
                    : new List<Core.Entities.Chat> { }
            );


            return Ok(apartmentDTO);
        }

        [HttpPost]
        [Authorize]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Post([FromForm] CreateApartmentForm form)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized("User has to be authorized before processing this operation.");
            }

            if (form.Images.Any() && form.Images.Count > 20)
            {
                return BadRequest("You cannot add more than 20 files.");
            }

            var ownerId = _contextAccessor.HttpContext!.User.GetUserId();
            var fullName = _contextAccessor.HttpContext!.User.GetFullName();
            var email = _contextAccessor.HttpContext!.User.GetEmail();
            var phoneNumber = _contextAccessor.HttpContext!.User.GetPhoneNumber();

            var images = new List<string>();

            if (form.Images is not null && form.Images.Any())
            {
                foreach (var file in form.Images)
                {
                    using var stream = file.OpenReadStream();                    
                    var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                    images.Add(resultForList.Url.ToString());                   
                }
            }
        
            var request = new CreateApartmentRequest
            (
                form.Title,
                form.Description,
                form.PriceForNight,
                new Address
                (
                    form.Country,
                    form.State,
                    form.City,
                    form.Street,
                    form.PostalCode
                ),              
                images,
                form.Telegram,
                form.Instagram
            );

            var result = await _sender.Send(new CreateApartmentCommand(ownerId, fullName, email, phoneNumber, request));

            return !result.IsSuccess ? BadRequest(result) : Ok(result);                      
        }

        [HttpPut("{id}")]
        [Authorize]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> Put(int id, [FromForm] UpdateApartmentForm form)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized("User has to be authorized before processing this operation.");
            }

            if (form.Images.Any() && form.Images.Count > 20)
            {
                return BadRequest("You cannot add more than 20 files.");
            }

            var thisUserId = _contextAccessor.HttpContext!.User.GetUserId();

            var images = new List<string>();

            if (form.Images is not null && form.Images.Any())
            {
                foreach (var file in form.Images)
                {
                    using var stream = file.OpenReadStream();
                    var resultForList = await _photoService.AddPhotoAsync(file.Name, stream);
                    images.Add(resultForList.Url.ToString());
                }
            }
            //string Title,
            //string Description,
            //decimal PriceForNight,
            //string Country,
            //string State,
            //string City,
            //string Street,
            //string PostalCode,
            //List< IFormFile > Images,
            //string? Telegram,
            //string? Instagram

            var request = new UpdateApartmentRequest
            (
                form.Title,
                form.Description,
                form.Price,

                new Address
                (
                    form.Country,
                    form.State,
                    form.City,
                    form.Street,
                    form.PostalCode
                ),
                
                images
            );

            var result = await _sender.Send(new UpdateApartmentCommand(id, thisUserId, request));
            return !result.IsSuccess ? BadRequest(result) : Ok(result);
        }

        //[HttpDelete("{id}")]
        //[Authorize(Roles = UserRole.Admin)]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var result = await _sender.Send(new DeleteApartmentCommand(id));

        //    if (!result.IsSuccess) return BadRequest(result);

        //    return Ok(result);
        //}
    }
}

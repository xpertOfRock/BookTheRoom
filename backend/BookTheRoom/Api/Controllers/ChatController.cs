using Api.Contracts.Chat;
using Application.UseCases.Commands.Chat;
using Application.UseCases.Queries.Chat;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatController(
            ISender sender,
            IHttpContextAccessor httpContextAccessor)
        {
            _sender = sender;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
        {
            var userId = _httpContextAccessor.HttpContext!.User.GetUserId() ?? string.Empty;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (!request.UserIds.Contains(userId)) return BadRequest();

            var result = await _sender.Send(new CreateChatCommand(request.UserIds, request.ApartmentId));

            return Ok(result);
        }
        [HttpGet("apartment-chats/{apartmentId}")]
        [Authorize]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> GetApartmentChatByUserId(int apartmentId)
        {
            var userId = _httpContextAccessor.HttpContext!.User.GetUserId() ?? string.Empty;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _sender.Send(new GetChatByUserIdQuery(userId, apartmentId));

            return Ok(result);
        }
        [HttpGet("{chatId}")]
        [Authorize]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> GetChatById(string chatId)
        {
            var userId = _httpContextAccessor.HttpContext!.User.GetUserId() ?? string.Empty;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var isSuccess = Guid.TryParse(chatId, out var resultChatId);

            if (!isSuccess) return BadRequest();

            var result = await _sender.Send(new GetChatByIdQuery(resultChatId));

            return Ok(result);
        }
    }
}

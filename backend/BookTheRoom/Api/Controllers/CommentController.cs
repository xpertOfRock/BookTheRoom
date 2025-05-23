using Api.Contracts.Comment;
using Application.UseCases.Commands.Comment;
using Application.UseCases.Queries.Comment;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IHttpContextAccessor _contextAccessor;

        public CommentController(
            ISender sender,
            IHttpContextAccessor contextAccessor)
        {
            _sender = sender;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        [Authorize]
        [EnableRateLimiting("SlidingModify")]
        public async Task<IActionResult> PostComment([FromBody] CreateCommentForm form)
        {
            var userId = _contextAccessor.HttpContext?.User.GetUserId();
            var username = _contextAccessor.HttpContext?.User.GetUsername();

            if (userId is null || string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            var result = await _sender.Send(new CreateCommentCommand(userId, username!, form.Description, form.PropertyId, form.PropertyType,form.UserScore));

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("user-comments")]
        [Authorize]
        [EnableRateLimiting("SlidingGet")]
        public async Task<IActionResult> GetUserComments([FromQuery] GetUserCommentsRequest request)
        {
            string? userId = _contextAccessor.HttpContext!.User.GetUserId() ?? string.Empty;

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _sender.Send(new GetUserCommentsQuery(userId, request));

            return Ok(result);
        }
    }
}

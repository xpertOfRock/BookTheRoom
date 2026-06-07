using Api.Contracts.Chat;
using Api.Controllers;
using Application.UseCases.Commands.Chat;
using Application.UseCases.Queries.Chat;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Tests.ControllersTests
{
    // Unit tests for ChatController.
    // Covers: missing userId → 401, userId not in participants → 400,
    // invalid GUID → 400, happy paths → 200.
    public class ChatControllerTests
    {
        private readonly Mock<IMediator> _sender = new();

        private ChatController BuildController(string? userId = "user-001")
        {
            var claims = new List<Claim>();
            if (userId != null)
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

            var identity = new ClaimsIdentity(claims,
                userId != null ? "TestAuth" : string.Empty);
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(a => a.HttpContext).Returns(httpContext);

            var controller = new ChatController(_sender.Object, accessorMock.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
            return controller;
        }

        private static readonly Chat SampleChat = new Chat
        {
            Id = Guid.NewGuid(),
            UsersId = new List<string> { "user-001" }
        };

        // POST /api/chat

        [Fact]
        public async Task CreateChat_WhenUserNotInParticipants_ReturnsBadRequest()
        {
            var request = new CreateChatRequest(
                UserIds: new List<string> { "other-user" },
                ApartmentId: 1);

            var result = await BuildController(userId: "user-001").CreateChat(request);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateChat_WhenUserIdMissing_ReturnsUnauthorized()
        {
            var request = new CreateChatRequest(
                UserIds: new List<string> { "user-001" },
                ApartmentId: 1);

            var result = await BuildController(userId: null).CreateChat(request);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task CreateChat_WhenUserInParticipants_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<CreateChatCommand>(), default))
                   .ReturnsAsync(SampleChat);

            var request = new CreateChatRequest(
                UserIds: new List<string> { "user-001", "user-002" },
                ApartmentId: 1);

            var result = await BuildController(userId: "user-001").CreateChat(request);

            Assert.IsType<OkObjectResult>(result);
        }

        // GET /api/chat/apartment-chats/{apartmentId}

        [Fact]
        public async Task GetApartmentChatByUserId_WhenUserIdMissing_ReturnsUnauthorized()
        {
            var result = await BuildController(userId: null)
                .GetApartmentChatByUserId(apartmentId: 1);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetApartmentChatByUserId_WhenAuthenticated_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetChatByUserIdQuery>(), default))
                   .ReturnsAsync(SampleChat);

            var result = await BuildController().GetApartmentChatByUserId(1);

            Assert.IsType<OkObjectResult>(result);
        }

        // GET /api/chat/{chatId}

        [Fact]
        public async Task GetChatById_WhenInvalidGuid_ReturnsBadRequest()
        {
            var result = await BuildController().GetChatById("not-a-guid");

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetChatById_WhenUserIdMissing_ReturnsUnauthorized()
        {
            var result = await BuildController(userId: null)
                .GetChatById(Guid.NewGuid().ToString());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetChatById_WithValidGuid_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetChatByIdQuery>(), default))
                   .ReturnsAsync(SampleChat);

            var result = await BuildController().GetChatById(Guid.NewGuid().ToString());

            Assert.IsType<OkObjectResult>(result);
        }
    }
}

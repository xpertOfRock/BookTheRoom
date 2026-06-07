using Api.Contracts.Room;
using Api.Controllers;
using Api.DTOs;
using Application.Interfaces;
using Application.UseCases.Commands.Room;
using Application.UseCases.Queries.Room;
using Core.Contracts;
using Core.Enums;
using Core.TasksResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tests.ControllersTests
{
    // Unit tests for RoomController.
    // Verifies routing logic: DTO mapping, guard clauses (null room),
    // and photo cleanup on mediator failure.
    public class RoomControllerTests
    {
        private readonly Mock<IMediator> _sender = new();
        private readonly Mock<IPhotoService> _photoService = new();
        private readonly RoomController _controller;

        public RoomControllerTests()
        {
            _controller = new RoomController(_sender.Object, _photoService.Object);
        }

        private static Core.Entities.Room MakeRoom(int number = 101) => new Core.Entities.Room
        {
            HotelId = 1,
            Number = number,
            Name = "Standard",
            Description = "desc",
            Price = 100m,
            Category = RoomCategory.OneBedApartments,
            Images = new List<string> { "https://example.com/img.jpg" }
        };

        private IFormFile MakeFile(string name = "img.jpg")
        {
            var stream = new MemoryStream();
            new StreamWriter(stream) { AutoFlush = true }.Write("data");
            stream.Position = 0;
            return new FormFile(stream, 0, stream.Length, "file", name)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }

        // GET /api/room/{hotelId}

        [Fact]
        public async Task GetAll_ReturnsMappedDTOs()
        {
            var rooms = new List<Core.Entities.Room> { MakeRoom(101), MakeRoom(102) };
            _sender.Setup(s => s.Send(It.IsAny<GetHotelRoomsQuery>(), default))
                   .ReturnsAsync(rooms);

            var result = await _controller.GetAll(1, new GetRoomsRequest(
                null, null, null, null, 0m, 500m,
                DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3)));

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GetRoomsResponse>(ok.Value);
            Assert.Equal(2, response.Rooms.Count);
        }

        [Fact]
        public async Task GetAll_WhenNoImages_MapsToNoImagePlaceholder()
        {
            var room = MakeRoom();
            room.Images = new List<string>();
            _sender.Setup(s => s.Send(It.IsAny<GetHotelRoomsQuery>(), default))
                   .ReturnsAsync(new List<Core.Entities.Room> { room });

            var result = await _controller.GetAll(1, new GetRoomsRequest(
                null, null, null, null, 0m, 500m,
                DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3)));

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GetRoomsResponse>(ok.Value);
            Assert.Equal("No Image", response.Rooms[0].Preview);
        }

        // GET /api/room/{hotelId}/{number}

        [Fact]
        public async Task Get_WhenRoomExists_ReturnsOkWithDTO()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetRoomQuery>(), default))
                   .ReturnsAsync(MakeRoom(101));

            var result = await _controller.Get(1, 101);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<RoomDTO>(ok.Value);
            Assert.Equal(101, dto.Number);
        }

        [Fact]
        public async Task Get_WhenRoomNotFound_ReturnsNotFound()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetRoomQuery>(), default))
                   .ReturnsAsync((Core.Entities.Room?)null);

            var result = await _controller.Get(1, 999);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // POST /api/room/{hotelId}

        [Fact]
        public async Task Post_WhenMediatorSucceeds_ReturnsOk()
        {
            _photoService.Setup(p => p.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                         .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult
                         { Url = new Uri("https://example.com/img.jpg") });

            _sender.Setup(s => s.Send(It.IsAny<CreateRoomCommand>(), default))
                   .ReturnsAsync(new Success("Room added."));

            var form = new CreateRoomForm("Standard", "desc", 101, 100m, 0,
                new List<IFormFile> { MakeFile() });

            var result = await _controller.Post(1, form);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Post_WhenMediatorFails_DeletesUploadedImagesAndReturnsBadRequest()
        {
            _photoService.Setup(p => p.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                         .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult
                         { Url = new Uri("https://example.com/img.jpg") });
            _photoService.Setup(p => p.DeletePhotoAsync(It.IsAny<string>()))
                         .ReturnsAsync(new CloudinaryDotNet.Actions.DeletionResult());

            _sender.Setup(s => s.Send(It.IsAny<CreateRoomCommand>(), default))
                   .ReturnsAsync(new Fail("Could not add room."));

            var form = new CreateRoomForm("Standard", "desc", 101, 100m, 0,
                new List<IFormFile> { MakeFile() });

            var result = await _controller.Post(1, form);

            Assert.IsType<BadRequestObjectResult>(result);
            _photoService.Verify(p => p.DeletePhotoAsync(It.IsAny<string>()), Times.Once);
        }

        // PUT /api/room/{hotelId}/{number}

        [Fact]
        public async Task Put_WhenMediatorSucceeds_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<UpdateRoomCommand>(), default))
                   .ReturnsAsync(new Success("Room updated."));

            var form = new UpdateRoomForm("Standard", "desc", 100m,
                RoomCategory.OneBedApartments, null);

            var result = await _controller.Put(1, 101, form);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Put_WhenMediatorFails_ReturnsBadRequest()
        {
            _sender.Setup(s => s.Send(It.IsAny<UpdateRoomCommand>(), default))
                   .ReturnsAsync(new Fail("Not found."));

            var form = new UpdateRoomForm("Standard", "desc", 100m,
                RoomCategory.OneBedApartments, null);

            var result = await _controller.Put(1, 999, form);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        // DELETE /api/room/{hotelId}/{number}

        [Fact]
        public async Task Delete_WhenSucceeds_ReturnsOk()
        {
            _sender.Setup(s => s.Send(It.IsAny<DeleteRoomCommand>(), default))
                   .ReturnsAsync(new Success("Deleted."));

            var result = await _controller.Delete(1, 101);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_WhenFails_ReturnsBadRequest()
        {
            _sender.Setup(s => s.Send(It.IsAny<DeleteRoomCommand>(), default))
                   .ReturnsAsync(new Fail("Room not found."));

            var result = await _controller.Delete(1, 999);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}

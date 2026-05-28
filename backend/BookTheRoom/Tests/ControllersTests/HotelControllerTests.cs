using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Commands.Hotel;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Api.Controllers;
using Api.Contracts.Hotel;
using Core.TasksResults;

namespace Tests.ControllersTests
{  
    public class HotelControllerTests
    {
        private readonly Mock<IPhotoService> _mockPhotoService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly HotelController _controller;

        public HotelControllerTests()
        {
            _mockPhotoService = new Mock<IPhotoService>();
            _mockMediator = new Mock<IMediator>();
            _controller = new HotelController(_mockMediator.Object, _mockPhotoService.Object);
        }
        [Fact]
        public async Task Put_WithInvalidData_ReturnsBadRequest()
        {
            // Uses 2 images (valid count) so the controller's guard clause does not
            // short-circuit before reaching the mediator. The invalid rating (-4)
            // is what triggers the Fail result returned by the mocked mediator.
            int hotelId = 1;
            var form = new UpdateHotelForm
            (
                "Updated Hotel",
                "Updated Description",
                -4,
                true,
                "Country",
                "State",
                "City",
                "Street",
                "PostalCode",
                new List<IFormFile>
                {
                    CreateMockFormFile("test1.jpg", "image/jpeg"),
                    CreateMockFormFile("test2.jpg", "image/jpeg")
                }
            );

            var uploadedImageUrls = new List<string> { "http://image1.com", "http://image2.com" };

            _mockPhotoService
                .SetupSequence(x => x.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[0]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[1]) });

            _mockMediator
                .Setup(x => x.Send(It.IsAny<UpdateHotelCommand>(), default))
                .ReturnsAsync(new Fail("Validation is failed.", Core.Enums.ErrorStatuses.ValidationError));

            var result = await _controller.Put(hotelId, form);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var actualResult = badRequestResult.Value as Fail;

            Assert.Equal("Validation is failed.", actualResult?.Message);
            Assert.Equal(false, actualResult?.IsSuccess);
            Assert.Equal(Core.Enums.ErrorStatuses.ValidationError, actualResult?.Status);
            _mockPhotoService.Verify(x => x.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()), Times.Exactly(2));
            _mockMediator.Verify(x => x.Send(It.IsAny<UpdateHotelCommand>(), default), Times.Once);
        }
        [Fact]
        public async Task Put_WithValidData_ReturnsOk()
        {
            int hotelId = 1;
            var form = new UpdateHotelForm
            (
                "Updated Hotel",
                "Updated Description",
                4,
                true,
                "Country",
                "State",
                "City",
                "Street",
                "PostalCode",
                new List<IFormFile>
                {
                    CreateMockFormFile("test1.jpg", "image/jpeg"),
                    CreateMockFormFile("test2.jpg", "image/jpeg")
                }
            );

            var uploadedImageUrls = new List<string> { "http://image1.com", "http://image2.com" };

            _mockPhotoService
                .SetupSequence(x => x.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()))
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[0]) })
                .ReturnsAsync(new CloudinaryDotNet.Actions.ImageUploadResult { Url = new Uri(uploadedImageUrls[1]) });

            _mockMediator
                .Setup(x => x.Send(It.IsAny<UpdateHotelCommand>(), default))
                .ReturnsAsync(new Success("Entity 'Hotel' was updated successfully."));

            var result = await _controller.Put(hotelId, form);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = okResult.Value as Success;
            Assert.Equal("Entity 'Hotel' was updated successfully.", actualResult?.Message);
            Assert.Equal(true, actualResult?.IsSuccess);
            _mockPhotoService.Verify(x => x.AddPhotoAsync(It.IsAny<string>(), It.IsAny<Stream>()), Times.Exactly(2));
            _mockMediator.Verify(x => x.Send(It.IsAny<UpdateHotelCommand>(), default), Times.Once);
        }

        private IFormFile CreateMockFormFile(string fileName, string contentType)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Fake image content");
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }

}

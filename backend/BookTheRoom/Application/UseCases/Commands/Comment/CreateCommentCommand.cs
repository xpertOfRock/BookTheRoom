using MediatR;

namespace Application.UseCases.Commands.Comment
{
    public class CreateCommentCommand : IRequest<Unit>
    {
        public int HotelId { get; set; }
        public string Description { get; set; }
        public CreateCommentCommand(int hotelId, string description)
        {
            HotelId = hotelId;
            Description = description;
        }
    }
}

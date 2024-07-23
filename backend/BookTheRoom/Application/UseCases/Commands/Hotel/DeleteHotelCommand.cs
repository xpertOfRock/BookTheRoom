using MediatR;

namespace Application.UseCases.Commands.Hotel
{
    public class DeleteHotelCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public DeleteHotelCommand(int id)
        {
            Id = id;
        }
    }
}

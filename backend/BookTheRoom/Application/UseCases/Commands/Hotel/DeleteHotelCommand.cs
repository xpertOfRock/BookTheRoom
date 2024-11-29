using Core.Interfaces;
using MediatR;

namespace Application.UseCases.Commands.Hotel
{
    public class DeleteHotelCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public DeleteHotelCommand(int id)
        {
            Id = id;
        }
    }
}

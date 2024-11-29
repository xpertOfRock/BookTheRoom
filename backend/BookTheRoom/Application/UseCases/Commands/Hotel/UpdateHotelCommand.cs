using Core.Interfaces;
using Core.Contracts;
using MediatR;

namespace Application.UseCases.Commands.Hotel
{
    public class UpdateHotelCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public UpdateHotelRequest UpdateHotelRequest { get; set; }
        
        public UpdateHotelCommand(int id, UpdateHotelRequest request)
        {
            Id = id;
            UpdateHotelRequest = request;
        }
    }
}

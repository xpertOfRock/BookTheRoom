using Application.UseCases.Abstractions;

namespace Application.UseCases.Commands.Hotel
{
    public class UpdateHotelCommand : ICommand<IResult>
    {
        public int Id { get; set; }
        public UpdateHotelRequest Request { get; set; }
        
        public UpdateHotelCommand(int id, UpdateHotelRequest request)
        {
            Id = id;
            Request = request;
        }
    }
}

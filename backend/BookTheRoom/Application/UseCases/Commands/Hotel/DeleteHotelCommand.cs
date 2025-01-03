using Application.UseCases.Abstractions;

namespace Application.UseCases.Commands.Hotel
{
    public class DeleteHotelCommand : ICommand<IResult>
    {
        public int Id { get; set; }
        public DeleteHotelCommand(int id)
        {
            Id = id;
        }
    }
}

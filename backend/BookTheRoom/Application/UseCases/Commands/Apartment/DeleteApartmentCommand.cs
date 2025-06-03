namespace Application.UseCases.Commands.Apartment
{
    public class DeleteApartmentCommand : ICommand<IResult>
    {
        public int Id { get; }
        public string UserId { get; }
        public DeleteApartmentCommand(int id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}

namespace Application.UseCases.Commands.Apartment
{
    public class UpdateUserDataInUserApartmentsCommand : ICommand<IResult>
    {
        public string UserId { get; }
        public UpdateUserDataInUserApartmentsRequest Request { get; }
        public UpdateUserDataInUserApartmentsCommand(string userId, UpdateUserDataInUserApartmentsRequest request)
        {
            UserId = userId;
            Request = request;
        }
    }
}

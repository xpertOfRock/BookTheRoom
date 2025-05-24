namespace Application.UseCases.Commands.Apartment
{
    public class CreateApartmentCommand : ICommand<IResult>
    {
        public string OwnerId { get; }
        public string OwnerName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public CreateApartmentRequest Request { get; }

        public CreateApartmentCommand(
            string ownerId,
            string ownerName,
            string email,
            string phoneNumber,
            CreateApartmentRequest request)
        {
            OwnerId = ownerId;
            OwnerName = ownerName;
            Email = email;
            PhoneNumber = phoneNumber;           
            Request = request;
        }
    }
}

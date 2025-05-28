namespace Core.Contracts
{
    public record UpdateUserDataInUserApartmentsRequest
    (
        string OwnerName,
        string Email,
        string PhoneNumber
    );
}

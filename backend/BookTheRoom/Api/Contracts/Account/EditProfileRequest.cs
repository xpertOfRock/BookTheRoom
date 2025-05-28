namespace Api.Contracts.Account
{
    public record EditProfileRequest
    (   
        string Email,
        string PhoneNumber,
        string FirstName,
        string LastName,
        IFormFile? Image
    );
}

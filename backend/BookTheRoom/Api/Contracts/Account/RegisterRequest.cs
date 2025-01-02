namespace Api.Contracts.Account
{
    public record RegisterRequest
    (
        string FirstName,
        string LastName,
        string Password,
        string Email,
        string Username,
        string PhoneNumber,
        DateOnly BirthDate
    );   
}

namespace Api.Contracts.Account
{
    public record RegisterRequest
    (
        string FirstName,
        string LastName,
        string Password,
        int Age,
        string Email,
        string Username,
        string PhoneNumber   
    );   
}

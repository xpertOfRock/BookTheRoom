namespace Api.Contracts.Account
{
    public record RegisterRequest
    (
        string FirstName,
        string LastName,
        string Password,
        int Age,
        string Email,        
        string PhoneNumber,
        string Country,
        string State,
        string City,
        string Street,
        string PostalCode,
        string? Username
    );   
}

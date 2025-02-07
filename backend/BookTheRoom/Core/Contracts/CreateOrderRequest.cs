namespace Core.Contracts
{
    public record CreateOrderRequest
    (
        string NonceFromClient,
        string Email,
        string Number,
        string FirstName,
        string LastName,
        bool MinibarIncluded,
        bool MealsIncluded,        
        DateTimeOffset CheckIn,
        DateTimeOffset CheckOut
    );
}
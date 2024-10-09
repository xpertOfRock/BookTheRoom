namespace Api.Contracts.Account
{
    public record AuthorizeRequest
    (
        string EmailOrUsername,
        string Password
    );
}

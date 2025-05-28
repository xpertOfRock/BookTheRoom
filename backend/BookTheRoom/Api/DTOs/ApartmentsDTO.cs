namespace Api.DTOs
{
    public record ApartmentsDTO
    (
        int Id,
        string Title,
        decimal Price,
        string Address,
        DateTime CreatedAt,
        string? Preview,
        float? UserScore
    );
}

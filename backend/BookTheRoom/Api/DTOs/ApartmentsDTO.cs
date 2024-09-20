namespace Api.DTOs
{
    public record ApartmentsDTO
    (
        int Id,
        string Title,
        string Address,
        string? Preview
    );
}

using Api.DTOs;

namespace Api.Contracts
{
    public record GetApartmentsResponse(List<ApartmentsDTO> Apartments);
}

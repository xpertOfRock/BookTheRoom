using Api.DTOs;

namespace Api.Contracts.Apartment
{
    public record GetApartmentsResponse(List<ApartmentsDTO> Apartments);
}

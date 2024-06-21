using MediatR;

namespace BookTheRoom.Application.UseCases.Queries.Address
{
    public class GetAllAddressesQuery : IRequest<List<Core.ValueObjects.Address>>
    {

    }
}

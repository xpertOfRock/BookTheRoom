using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Address
{
    public class DeleteAddressCommand : IRequest<Unit>
    {
        public Core.ValueObjects.Address Address { get; set; }
        public DeleteAddressCommand(Core.ValueObjects.Address address)
        {
            Address = address;
        }
    }
}

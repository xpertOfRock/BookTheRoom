using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Address
{
    public class CreateAddressCommand : IRequest<Unit>
    {
        public Core.ValueObjects.Address Address { get; set; }
        public CreateAddressCommand(Core.ValueObjects.Address address)
        {
            Address = address;
        }

    }
}

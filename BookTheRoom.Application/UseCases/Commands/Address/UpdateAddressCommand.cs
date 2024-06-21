using MediatR;

namespace BookTheRoom.Application.UseCases.Commands.Address
{
    public class UpdateAddressCommand : IRequest<Unit>
    {
        public Core.ValueObjects.Address Address { get; set; }
        public UpdateAddressCommand(Core.ValueObjects.Address address)
        {
            Address = address;
        }
    }
}

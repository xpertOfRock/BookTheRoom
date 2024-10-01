using Core.Contracts;
using Core.ValueObjects;
using MediatR;

namespace Application.UseCases.Commands.Apartment
{
    public class CreateApartmentCommand : IRequest<Unit>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public decimal Price { get; set; }

        public Address Address { get; set; }
        public List<string > Images { get; set; }
        public CreateApartmentCommand(CreateApartmentRequest request)
        {
            Title = request.Title;
            Description = request.Description;
            OwnerId = request.OwnerId;
            OwnerName = request.OwnerName;
            Price = request.Price;
            Address = request.Address;
            Images = request.Images;
        }
    }
}

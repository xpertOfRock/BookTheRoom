using Application.Interfaces;
using Application.UseCases.Commands.Apartment;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Apartment
{
    public class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateApartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Apartments.Add
            (
                new Core.Entities.Apartment
                {
                    Title = request.Title,
                    Description = request.Description,
                    OwnerId = request.OwnerId,
                    PriceForNight = request.Price,
                    Address = request.Address,
                    Images = request.Images,
                    Comments = new List<Core.Entities.Comment>()
                }
            );
            return Unit.Value;
        }
    }
}

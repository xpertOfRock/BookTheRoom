using Core.Interfaces;
using Application.UseCases.Commands.Apartment;
using MediatR;
using Application.Interfaces;
using Core.TasksResults;

namespace Application.UseCases.Handlers.CommandHandlers.Apartment
{
    public class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateApartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IResult> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var result =  await _unitOfWork.Apartments.Add
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

                if (!result.IsSuccess)
                {
                    await _unitOfWork.RollbackAsync();
                    return result;
                }

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the apartment.", ex);
            }
        }
    }
}

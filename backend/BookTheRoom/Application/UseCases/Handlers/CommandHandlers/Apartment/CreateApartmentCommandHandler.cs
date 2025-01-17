using Application.UseCases.Commands.Apartment;

namespace Application.UseCases.Handlers.CommandHandlers.Apartment
{
    public class CreateApartmentCommandHandler : ICommandHandler<CreateApartmentCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateApartmentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IResult> Handle(CreateApartmentCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var result =  await _unitOfWork.Apartments.Add
                (
                    new Core.Entities.Apartment
                    {
                        Title = command.Title,
                        Description = command.Description,
                        OwnerId = command.OwnerId,
                        PriceForNight = command.Price,
                        Address = command.Address,
                        Images = command.Images,
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

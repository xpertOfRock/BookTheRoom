using Application.UseCases.Commands.Apartment;

namespace Application.UseCases.Handlers.CommandHandlers.Apartment
{
    public class CreateApartmentCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateApartmentCommand, IResult>
    {

        public async Task<IResult> Handle(CreateApartmentCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var result =  await unitOfWork.Apartments.Add
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
                    },
                    cancellationToken
                );

                if (!result.IsSuccess)
                {
                    await unitOfWork.RollbackAsync();
                    return result;
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the apartment.", ex);
            }
        }
    }
}

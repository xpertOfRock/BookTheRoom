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
                var apartment = new Core.Entities.Apartment
                {
                    OwnerId = command.OwnerId,
                    OwnerName = command.OwnerName,
                    Email = command.Email,
                    PhoneNumber = command.PhoneNumber,
                    Telegram = command.Request.Telegram,
                    Instagram = command.Request.Instagram,
                    Title = command.Request.Title,
                    Description = command.Request.Description,
                    PriceForNight = command.Request.Price,
                    Address = command.Request.Address,
                    Images = command.Request.Images
                };

                var result = await unitOfWork.Apartments.Add(apartment, cancellationToken);

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

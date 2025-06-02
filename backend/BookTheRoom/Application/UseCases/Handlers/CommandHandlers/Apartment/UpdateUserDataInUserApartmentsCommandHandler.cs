using Application.UseCases.Commands.Apartment;

namespace Application.UseCases.Handlers.CommandHandlers.Apartment
{
    public class UpdateUserDataInUserApartmentsCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserDataInUserApartmentsCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateUserDataInUserApartmentsCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var result = await unitOfWork.Apartments.UpdateUserDataInUserApartments(command.UserId, command.Request, cancellationToken);

                await unitOfWork.CommitAsync();

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the apartment(-s).", ex);
            }
        }
    }
}

using Application.UseCases.Commands.Apartment;

namespace Application.UseCases.Handlers.CommandHandlers.Apartment
{
    public class DeleteApartmentCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteApartmentCommand, IResult>
    {
        public async Task<IResult> Handle(DeleteApartmentCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var result = await unitOfWork.Apartments.Delete(command.Id, command.UserId, cancellationToken);

                if (!result.IsSuccess)
                {
                    await unitOfWork.RollbackAsync();
                    return result;
                }

                await unitOfWork.SaveChangesAsync();

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

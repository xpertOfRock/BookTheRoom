using Application.UseCases.Commands.Apartment;

namespace Application.UseCases.Handlers.CommandHandlers.Apartment
{
    class UpdateApartmentCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateApartmentCommand, IResult>
    {
        public async Task<IResult> Handle(UpdateApartmentCommand command, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {
                var result = await unitOfWork.Apartments.Update(command.Id, command.UserId, command.Request);

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

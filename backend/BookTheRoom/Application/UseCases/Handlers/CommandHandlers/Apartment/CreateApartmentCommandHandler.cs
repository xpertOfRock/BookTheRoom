using Application.UseCases.Commands.Apartment;

namespace Application.UseCases.Handlers.CommandHandlers.Apartment
{
    public class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateApartmentCommand> _validator;
        public CreateApartmentCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateApartmentCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<IResult> Handle(CreateApartmentCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var validationResult = _validator.Validate(command);

                if (!validationResult.IsValid)
                {
                    return new Fail("Validation is failed.", Core.Enums.ErrorStatuses.ValidationError);
                }

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

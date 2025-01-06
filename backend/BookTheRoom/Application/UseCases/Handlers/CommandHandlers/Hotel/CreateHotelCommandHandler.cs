using Application.UseCases.Abstractions;
using Application.UseCases.Commands.Hotel;

namespace Application.UseCases.Handlers.CommandHandlers.Hotel
{
    public class CreateHotelCommandHandler : ICommandHandler<CreateHotelCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateHotelCommand> _validator;
        public CreateHotelCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateHotelCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<IResult> Handle(CreateHotelCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return new Fail("Validation is failed.", Core.Enums.ErrorStatuses.ValidationError);
                }

                var result = await _unitOfWork.Hotels.Add
                (
                    new Core.Entities.Hotel
                    {
                        Name = command.Name,
                        Description = command.Description,
                        Rating = command.Rating,
                        HasPool = command.HasPool,
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
                throw new InvalidOperationException("An error occurred while processing the hotel.", ex);
            }
        }
    }
}

using Application.Interfaces;
using Application.UseCases.Commands.Hotel;
using Application.UseCases.Commands.Room;
using Core.Contracts;
using Core.Interfaces;
using Core.TasksResults;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Room
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateRoomCommand> _validator;
        public CreateRoomCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateRoomCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<IResult> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var validationResult = _validator.Validate(command);

                if (!validationResult.IsValid)
                {
                    return new Fail("Validation is failed.", Core.Enums.ErrorStatuses.ValidationError);
                }

                var hotel = await _unitOfWork.Hotels.GetById(command.HotelId);

                if (hotel is null)
                {
                    await _unitOfWork.RollbackAsync();
                    return new Fail("Hotel not found.");
                }

                var result1 = await _unitOfWork.Rooms.Add
                (
                    new Core.Entities.Room
                    {
                        Name = command.Name,
                        Description = command.Description,
                        Number = command.Number,
                        Price = command.Price,
                        Category = command.Category,
                        HotelId = command.HotelId,
                    }
                );

                var room = await _unitOfWork.Rooms.GetById(command.HotelId, command.Number);

                if (room is null)
                {
                    await _unitOfWork.RollbackAsync();
                    return new Fail("Room not found after creation.");
                }

                var images = command.Images.Any() ? command.Images : hotel.Images;

                hotel.Rooms.Add(room);

                var result2 = await _unitOfWork.Hotels.Update
                    (command.HotelId,
                        new UpdateHotelRequest
                        (
                            hotel.Name,
                            hotel.Description,
                            hotel.Rating,
                            hotel.HasPool,
                            images
                        )
                    );

                IResult result = (!result1.IsSuccess || !result2.IsSuccess) 
                               ? new Fail("Could not add room to the list of hotel rooms.") 
                               : new Success("Room was successfuly added to the list of hotel rooms.");

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
                throw new InvalidOperationException("An error occurred while processing the room.", ex);
            }
        }
    }
}

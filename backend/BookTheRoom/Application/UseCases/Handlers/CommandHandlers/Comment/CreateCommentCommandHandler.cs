using Application.Interfaces;
using Application.UseCases.Commands.Comment;
using Core.Interfaces;
using Core.TasksResults;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Comment
{   
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, IResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IResult> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                IResult result = new Fail("Variable that represents property type for the eentity 'Comment' was not passed as the parameter in method.");

                var comment = new Core.Entities.Comment
                {
                    Description = request.Description,
                    HotelId = request.HotelId,
                    ApartmentId = request.ApartmentId,
                    PropetyCategory = request.PropertyCategory,
                    CreatedAt = request.CreatedAt,
                    UpdatedAt = request.UpdatedAt
                };

                switch (request.PropertyCategory)
                {
                    case Core.Enums.PropertyCategory.Hotel:

                        var hotel = await _unitOfWork.Hotels.GetById(request.HotelId);

                        if (hotel is null)
                        {
                            await _unitOfWork.RollbackAsync();
                            return new Fail("Impossible to add comment to a non-existent hotel.");
                        }

                        result = await _unitOfWork.Comments.Add(comment);

                        hotel.Comments?.Add(comment);
                        break;

                    case Core.Enums.PropertyCategory.Apartment:

                        var apartment = await _unitOfWork.Apartments.GetById(request.ApartmentId);

                        if (apartment is null)
                        {
                            await _unitOfWork.RollbackAsync();
                            return new Fail("Impossible to add comment to a non-existent apartments.");
                        }

                        result = await _unitOfWork.Comments.Add(comment);

                        apartment.Comments?.Add(comment);
                        break;
                }
               
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the comment.", ex);
            }
        }
    }
}

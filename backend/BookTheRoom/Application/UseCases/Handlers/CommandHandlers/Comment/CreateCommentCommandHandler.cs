using Application.Interfaces;
using Application.UseCases.Commands.Comment;
using Core.Entities;
using MediatR;

namespace Application.UseCases.Handlers.CommandHandlers.Comment
{   
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
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

                        if (hotel == null)
                        {
                            return Unit.Value;
                        }

                        await _unitOfWork.Comments.Add(comment);

                        hotel.Comments?.Add(comment);
                        break;

                    case Core.Enums.PropertyCategory.Apartment:

                        var apartment = await _unitOfWork.Apartments.GetById(request.ApartmentId);

                        if (apartment == null)
                        {
                            return Unit.Value;
                        }

                        await _unitOfWork.Comments.Add(comment);

                        apartment.Comments?.Add(comment);
                        break;
                }
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the comment.", ex);
            }
            return Unit.Value;
        }
    }
}

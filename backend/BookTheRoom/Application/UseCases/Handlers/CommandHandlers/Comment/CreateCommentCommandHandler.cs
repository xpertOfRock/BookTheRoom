using Application.UseCases.Abstractions;
using Application.UseCases.Commands.Comment;

namespace Application.UseCases.Handlers.CommandHandlers.Comment
{   
    public class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, IResult>
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
                var hotel = await _unitOfWork.Hotels.GetById(request.HotelId);
                var apartment = await _unitOfWork.Apartments.GetById(request.ApartmentId);

                var comment = new Core.Entities.Comment
                {
                    UserId = request.UserId,
                    Username = request.Username,
                    Description = request.Description,
                    HotelId = request.HotelId,
                    ApartmentId = request.ApartmentId,
                    CreatedAt = request.CreatedAt,
                    UpdatedAt = request.UpdatedAt
                };
                                   

                var result = await _unitOfWork.Comments.Add(comment);

                if (hotel is not null)
                {
                    hotel.Comments?.Add(comment);
                    await _unitOfWork.Hotels.UpdateCache(hotel); 
                }
                else if (apartment is not null)
                {
                    apartment.Comments?.Add(comment);
                    await _unitOfWork.Apartments.UpdateCache(apartment);
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

using Application.UseCases.Commands.Comment;

namespace Application.UseCases.Handlers.CommandHandlers.Comment
{   
    public class CreateCommentCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateCommentCommand, IResult>
    {
        public async Task<IResult> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.BeginTransactionAsync();

            try
            {      
                var hotel = await unitOfWork.Hotels.GetById(request.HotelId);
                var apartment = await unitOfWork.Apartments.GetById(request.ApartmentId);

                var comment = new Core.Entities.Comment
                {
                    UserId = request.UserId,
                    Username = request.Username,
                    Description = request.Description,
                    UserScore = request.UserScore ?? null,
                    HotelId = request.HotelId,
                    ApartmentId = request.ApartmentId,
                    CreatedAt = request.CreatedAt,
                    UpdatedAt = request.UpdatedAt
                };
                                   

                var result = await unitOfWork.Comments.Add(comment);

                if (result is null) 
                {
                    await unitOfWork.RollbackAsync();
                    return new Fail("Failed to add a new entity 'Comment' to the entity 'Hotel'.");
                }

                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitAsync();

                if (hotel is not null)
                {
                    hotel.Comments?.Add(comment);
                    await unitOfWork.Hotels.UpdateCache(hotel); 
                }
                else if (apartment is not null)
                {
                    apartment.Comments?.Add(comment);
                    await unitOfWork.Apartments.UpdateCache(apartment);
                }
               
                return result;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                throw new InvalidOperationException("An error occurred while processing the comment.", ex);
            }
        }
    }
}

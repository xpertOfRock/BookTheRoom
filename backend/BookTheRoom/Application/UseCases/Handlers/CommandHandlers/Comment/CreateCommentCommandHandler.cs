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
                var comment = new Core.Entities.Comment
                {
                    UserId = request.UserId,
                    Username = request.Username,
                    Description = request.Description,
                    UserScore = request.UserScore ?? null,
                    HotelId = request.PropertyType == PropertyType.Hotel ? request.PropertyId : null,
                    ApartmentId = request.PropertyType == PropertyType.Apartment ? request.PropertyId : null,
                    CreatedAt = request.CreatedAt,
                    UpdatedAt = request.UpdatedAt
                };                                   

                var result = await unitOfWork.Comments.Add(comment);

                if (result is null) 
                {
                    await unitOfWork.RollbackAsync();
                    return new Fail("Failed to add a new entity 'Comment' to the entity 'Hotel'.");
                }                

                if (request.PropertyType == PropertyType.Hotel)
                {
                    var hotel = await unitOfWork.Hotels.GetById(request.PropertyId);

                    hotel!.Comments?.Add(comment);
                    await unitOfWork.Hotels.UpdateCache(hotel); 
                }
                else if (request.PropertyType == PropertyType.Apartment)
                {
                    var apartment = await unitOfWork.Apartments.GetById(request.PropertyId);

                    apartment!.Comments?.Add(comment);
                    await unitOfWork.Apartments.UpdateCache(apartment);
                }

                await unitOfWork.SaveChangesAsync();

                await unitOfWork.CommitAsync();

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

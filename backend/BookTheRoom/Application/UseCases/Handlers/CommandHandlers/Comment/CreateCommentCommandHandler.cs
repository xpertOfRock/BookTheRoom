using Application.UseCases.Commands.Comment;

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
    //            public record UpdateHotelRequest
    //            (
    //               string Name,
    //               string Description,
    //               int Rating,
    //               bool HasPool,
    //               Address Address,
    //               List<string>? Images
    //            );
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
                    await _unitOfWork.Hotels.Update(hotel.Id, new UpdateHotelRequest(hotel.Name, hotel.Description, hotel.Rating, hotel.HasPool, hotel.Address, hotel.Images)); //to clear cache value
                }
                else if (apartment is not null)
                {
                    await _unitOfWork.Apartments.Update(apartment.Id, new UpdateApartmentRequest(apartment.Title, apartment.Description, apartment.PriceForNight, apartment.Images)); //to clear cache value
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

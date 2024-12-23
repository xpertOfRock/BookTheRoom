namespace Application.UseCases.Commands.Comment
{
    public class CreateCommentCommand : IRequest<IResult>
    {
        public string UserId { get; set; }
        public int? HotelId { get; set; }
        public int? ApartmentId { get; set; }
        public string Description { get; set; }
        public PropertyCategory PropertyCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CreateCommentCommand
        (
            string userId,
            CreateCommentRequest request,
            int? hotelId = null,
            int? apartmentId = null            
        )
        {
            UserId = userId;
            HotelId = hotelId;
            ApartmentId = apartmentId;
            Description = request.Description;
            PropertyCategory = request.PropertyCategory;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = null;
        }
    }
}

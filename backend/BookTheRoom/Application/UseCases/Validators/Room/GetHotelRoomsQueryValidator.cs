using Application.UseCases.Queries.Room;

namespace Application.UseCases.Validators.Room
{
    public class GetHotelRoomsQueryValidator : AbstractValidator<GetHotelRoomsQuery>
    {
        public GetHotelRoomsQueryValidator()
        {
            RuleFor(x => x.CheckIn)
                .GreaterThan(x => DateTime.UtcNow.AddDays(1)).WithMessage("CheckIn value must be greater than today's date and time with added one day (in UTC).");

            RuleFor(x => x.CheckOut)
                .GreaterThanOrEqualTo(x => x.CheckIn.AddDays(1))
                .WithMessage("CheckOut value must be greater than CheckIn with added one day (in UTC).");
        }
    }
}

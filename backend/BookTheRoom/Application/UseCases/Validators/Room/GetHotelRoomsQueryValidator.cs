using Application.UseCases.Queries.Room;

namespace Application.UseCases.Validators.Room
{
    public class GetHotelRoomsQueryValidator : AbstractValidator<GetHotelRoomsQuery>
    {
        public GetHotelRoomsQueryValidator()
        {
            RuleFor(x => x.CheckIn!.Value.Date)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date.AddDays(1)).WithMessage("CheckIn value must be greater than today's date and time with added one day (in UTC).");

            RuleFor(x => x.CheckOut!.Value.Date)
                .GreaterThanOrEqualTo(x => x.CheckIn!.Value.AddDays(1))
                .WithMessage("CheckOut value must be greater than CheckIn with added one day (in UTC).");

            RuleFor(x => x.Request.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min price can only be positive value.");

            RuleFor(x => x.Request.MaxPrice)
                .GreaterThan(x => x.Request.MinPrice)
                .WithMessage("Min price can only be positive value.");
        }
    }
}

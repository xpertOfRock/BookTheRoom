using Application.UseCases.Commands.Order;
using Application.UseCases.Validators.Address;

namespace Application.UseCases.Validators.Order
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Request.CheckIn)
                .NotEmpty()
                    .WithMessage("CheckIn is required.")
                .LessThan(x => x.Request.CheckOut)
                    .WithMessage("CheckIn can't be greater than CheckOut");

            RuleFor(x => x.Request.CheckOut)
                .NotEmpty()
                    .WithMessage("CheckOut is required.")
                .GreaterThanOrEqualTo(x => x.Request.CheckOut.AddDays(1))
                    .WithMessage("CheckOut can't be less than CheckIn");

            RuleFor(x => x.Request.Number)
                .NotEmpty()
                    .WithMessage("Phone number is required.");

            RuleFor(x => x.Request.Email)
                .EmailAddress()
                    .WithMessage("Email must be valid")
                .NotEmpty()
                    .WithMessage("Email number is required.");

            RuleFor(x => x.Request.FirstName)
                .NotEmpty()
                    .WithMessage("First name is required.");

            RuleFor(x => x.Request.LastName)
                .NotEmpty()
                    .WithMessage("Last name is required.");

            RuleFor(x => x.Request.NonceFromClient)
                .NotEmpty()
                    .WithMessage("NonceString with encoded credit card and order data is required.");

            RuleFor(x => x.Request.MealsIncluded)
                 .NotNull()
                    .WithMessage("MealsIncluded can be either TRUE or FALSE");

            RuleFor(x => x.Request.MinibarIncluded)
                 .NotNull()
                    .WithMessage("MinibarIncluded can be either TRUE or FALSE");

            RuleFor(x => x.UserId)
                .NotEmpty()
                    .WithMessage("UserId is required.");

            RuleFor(x => x.Number)                
                .NotEmpty()
                    .WithMessage("Room number is required.");

            RuleFor(x => x.HotelId)
                .NotEmpty()
                    .WithMessage("HotelId is required.");
        }
    }
}

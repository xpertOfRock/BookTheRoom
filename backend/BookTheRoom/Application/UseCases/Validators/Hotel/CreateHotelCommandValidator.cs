using Application.UseCases.Commands.Hotel;
using Application.UseCases.Validators.Address;

namespace Application.UseCases.Validators.Hotel
{
    public sealed class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
    {
        public CreateHotelCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                    .WithMessage("The hotel name cannot be empty.")
                .MaximumLength(100)
                    .WithMessage("The hotel name must not exceed 100 characters.");

            RuleFor(c => c.Description)
                .NotEmpty()
                    .WithMessage("The description cannot be empty.")
                .MaximumLength(3000)
                    .WithMessage("The description must not exceed 3000 characters.");

            RuleFor(c => c.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("The rating must be between 1 and 5.");

            RuleFor(c => c.Address)
                .NotNull()
                    .WithMessage("The address is required.")
                .SetValidator(new AddressValidator());

            RuleFor(x => x.Images)
                .NotNull()
                    .WithMessage("Images are required.")
                .Must(images => images!.Count <= 20)
                    .WithMessage("You can upload up to 20 images.")
                .ForEach(image => image.Must(img => Uri.IsWellFormedUriString(img, UriKind.Absolute))
                    .WithMessage("Each image URL must be a valid URL."));
        }
    }   
}

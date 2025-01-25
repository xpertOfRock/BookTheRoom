using Application.UseCases.Commands.Hotel;

namespace Application.UseCases.Validators.Hotel
{
    public sealed class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
    {
        public UpdateHotelCommandValidator()
        {
            RuleFor(c => c.Request.Name)
                .NotEmpty()
                    .WithMessage("The hotel name cannot be empty.")
                .MaximumLength(100)
                    .WithMessage("The hotel name must not exceed 100 characters.");

            RuleFor(c => c.Request.Description)
                .NotEmpty()
                    .WithMessage("The description cannot be empty.")
                .MaximumLength(3000)
                    .WithMessage("The description must not exceed 3000 characters.");

            RuleFor(c => c.Request.Rating)
                .InclusiveBetween(1, 5)
                    .WithMessage("The rating must be between 1 and 5.");

            RuleFor(c => c.Request.Images)
                .Must(images => images == null || images.Count <= 20)
                    .WithMessage("The number of images must not exceed 10.");

            RuleFor(x => x.Request.Images)
                .Must(images => images == null || images.Count <= 20)
                    .WithMessage("You can upload up to 20 images.")
                .When(x => x.Request.Images != null)
                .ForEach(image => image.Must(img => Uri.IsWellFormedUriString(img, UriKind.Absolute))
                    .WithMessage("Each image URL must be a valid URL."));
        }
    }
}

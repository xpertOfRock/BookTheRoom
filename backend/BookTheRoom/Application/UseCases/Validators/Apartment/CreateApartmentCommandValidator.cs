using Application.UseCases.Commands.Apartment;
using Application.UseCases.Validators.Address;

namespace Application.UseCases.Validators.Apartment
{
    public class CreateApartmentCommandValidator : AbstractValidator<CreateApartmentCommand>
    {
        public CreateApartmentCommandValidator()
        {
            RuleFor(x => x.Request.Title)
                .NotEmpty()
                    .WithMessage("Title is required.")
                .Length(3, 100)
                    .WithMessage("Title must be between 3 and 100 characters.");

            RuleFor(x => x.Request.Description)
                .NotEmpty()
                    .WithMessage("Description is required.")
                .MaximumLength(500)
                    .WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.OwnerId)
                .NotEmpty()
                    .WithMessage("OwnerId is required.");

            RuleFor(x => x.OwnerName)
                .NotEmpty()
                    .WithMessage("OwnerName is required.")
                .Length(3, 100)
                    .WithMessage("Owner name must be between 3 and 100 characters.");

            RuleFor(x => x.Request.Price)
                .GreaterThan(0)
                    .WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Request.Telegram)
                .Must(x => x[0].ToString() is "@")
                    .WithMessage("Enter valid telegram username.");
                
            RuleFor(x => x.Request.Instagram)
                .Must(x => Uri.IsWellFormedUriString(x, UriKind.Absolute))
                    .WithMessage("Instagram URL must be a valid URL.");

            RuleFor(x => x.Email)
                .EmailAddress()
                    .WithMessage("Email must be a valid Email.");

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .WithMessage("Phone number can't be empty.");

            RuleFor(x => x.Request.Address)
                .NotNull()
                    .WithMessage("Address is required.")
                .SetValidator(new AddressValidator());

            RuleFor(x => x.Request.Images)
                .NotNull()
                    .WithMessage("Images are required.")
                .Must(images => images!.Count <= 20)
                    .WithMessage("You can upload up to 10 images.")
                .ForEach(image => image.Must(img => Uri.IsWellFormedUriString(img, UriKind.Absolute))
                    .WithMessage("Each image URL must be a valid URL."));
        }
    }

}

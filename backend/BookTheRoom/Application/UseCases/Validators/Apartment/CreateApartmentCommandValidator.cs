using Application.UseCases.Commands.Apartment;
using Application.UseCases.Validators.Address;
using FluentValidation;

namespace Application.UseCases.Validators.Apartment
{
    public class CreateApartmentCommandValidator : AbstractValidator<CreateApartmentCommand>
    {
        public CreateApartmentCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("OwnerId is required.");

            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("OwnerName is required.")
                .Length(3, 100).WithMessage("Owner name must be between 3 and 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required.")
                .SetValidator(new AddressValidator());

            RuleFor(x => x.Images)
                .NotNull().WithMessage("Images are required.")
                .Must(images => images!.Count <= 20).WithMessage("You can upload up to 10 images.")
                .ForEach(image => image.Must(img => Uri.IsWellFormedUriString(img, UriKind.Absolute))
                    .WithMessage("Each image URL must be a valid URL."));
        }
    }

}

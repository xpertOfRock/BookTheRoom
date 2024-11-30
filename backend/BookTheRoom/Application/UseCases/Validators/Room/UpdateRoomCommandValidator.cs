using Application.UseCases.Commands.Room;
using FluentValidation;

namespace Application.UseCases.Validators.Room
{
    public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
    {
        public UpdateRoomCommandValidator()
        {
            RuleFor(x => x.HotelId)
                .GreaterThan(0).WithMessage("HotelId must be greater than 0.");

            RuleFor(x => x.Number)
                .GreaterThan(0).WithMessage("Room number must be greater than 0.");

            RuleFor(x => x.Request)
                .NotNull().WithMessage("Room request details cannot be null.");

            RuleFor(x => x.Request.Name)
                .NotEmpty().WithMessage("Room name cannot be empty.")
                .Length(3, 100).WithMessage("Room name must be between 3 and 100 characters.");

            RuleFor(x => x.Request.Description)
                .NotEmpty().WithMessage("Room description cannot be empty.")
                .MaximumLength(3000).WithMessage("Room description cannot exceed 500 characters.");

            RuleFor(x => x.Request.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Request.Category)
                .IsInEnum().WithMessage("Invalid room category.");

            RuleFor(x => x.Request.Images)
                .Must(images => images == null || images.Count <= 20).WithMessage("You can upload up to 20 images.")
                .When(x => x.Request.Images != null)
                .ForEach(image => image.Must(img => Uri.IsWellFormedUriString(img, UriKind.Absolute))
                    .WithMessage("Each image URL must be a valid URL."));

            RuleFor(x => x.Request.IsFree)
                .NotNull().WithMessage("IsFree flag cannot be null.");
        }
    }
}

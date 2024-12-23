using Application.UseCases.Commands.Room;

namespace Application.UseCases.Validators.Room
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Room name is required.")
                .MaximumLength(100).WithMessage("Room name must not exceed 100 characters.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Room description is required.")
                .MaximumLength(3000).WithMessage("Room description must not exceed 1000 characters.");

            RuleFor(c => c.Number)
                .GreaterThan(0).WithMessage("Room number must be greater than 0.");

            RuleFor(c => c.Price)
                .GreaterThan(0).WithMessage("Room price must be greater than 0.");

            RuleFor(c => c.HotelId)
                .GreaterThan(0).WithMessage("HotelId must be a positive integer.");

            RuleFor(c => c.Category)
                .IsInEnum().WithMessage("Invalid room category.");

            RuleFor(x => x.Images)
                .NotNull().WithMessage("Images are required.")
                .Must(images => images!.Count <= 20).WithMessage("You can upload up to 10 images.")
                .ForEach(image => image.Must(img => Uri.IsWellFormedUriString(img, UriKind.Absolute))
                    .WithMessage("Each image URL must be a valid URL."));
        }
    }
}

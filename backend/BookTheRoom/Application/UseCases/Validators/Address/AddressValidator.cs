namespace Application.UseCases.Validators.Address
{
    public class AddressValidator : AbstractValidator<Core.ValueObjects.Address>
    {
        public AddressValidator()
        {
            RuleFor(a => a.Country)
                .NotEmpty()
                .WithMessage("Field 'Country' cannot be empty.")
                .MaximumLength(100)
                .WithMessage("Field 'Country' cannot contain more than 100 symbols."); ;

            RuleFor(a => a.State)
                .NotEmpty()
                .WithMessage("Field 'State' cannot be empty.")
                .MaximumLength(100)
                .WithMessage("Field 'State' cannot contain more than 100 symbols.");

            RuleFor(a => a.City)
                .NotEmpty()
                .WithMessage("Field 'City' cannot be empty.")
                .MaximumLength(100)
                .WithMessage("Field 'City' cannot contain more than 100 symbols."); ;

            RuleFor(a => a.Street)
                .NotEmpty()
                .WithMessage("Field 'Street' cannot be empty.")
                .MaximumLength(200)
                .WithMessage("Field 'Street' cannot contain more than 200 symbols.");

            RuleFor(a => a.PostalCode)
                .NotEmpty()
                .WithMessage("Field 'PostalCode' cannot be empty.")
                .MaximumLength(20)
                .WithMessage("Field 'PostalCode' cannot contain more than 20 symbols.");
        }
    }
}

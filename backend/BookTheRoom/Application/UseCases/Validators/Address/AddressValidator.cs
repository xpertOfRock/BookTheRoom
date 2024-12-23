namespace Application.UseCases.Validators.Address
{
    public class AddressValidator : AbstractValidator<Core.ValueObjects.Address>
    {
        public AddressValidator()
        {
            RuleFor(a => a.Country).NotEmpty().WithMessage("Field 'Country' cannot be empty.");
            RuleFor(a => a.State).NotEmpty().WithMessage("Field 'State' cannot be empty.");
            RuleFor(a => a.City).NotEmpty().WithMessage("Field 'City' cannot be empty.");
            RuleFor(a => a.Street).NotEmpty().WithMessage("Field 'Street' cannot be empty.");
            RuleFor(a => a.PostalCode).NotEmpty().WithMessage("Field 'PostalCode' cannot be empty.");
        }
    }
}

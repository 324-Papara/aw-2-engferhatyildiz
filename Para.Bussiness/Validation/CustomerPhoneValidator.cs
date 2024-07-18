using FluentValidation;
using Para.Data.Domain;

namespace Para.Bussiness.Validation;

public class CustomerPhoneValidator : AbstractValidator<CustomerPhone>
{
    public CustomerPhoneValidator()
    {
        RuleFor(x => x.CountyCode)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Country Code must be at least 3 characters long!");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Length(10)
            .WithMessage("Phone number must be exactly 10 characters long!");
    }
}
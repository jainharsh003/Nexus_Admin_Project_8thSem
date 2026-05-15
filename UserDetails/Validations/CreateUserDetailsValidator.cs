using FluentValidation;
using UserDetails.DTOs;

namespace UserDetails.Validations
{
    public class CreateUserDetailsValidator : AbstractValidator<CreateUserDetailsDto>
    {
        public CreateUserDetailsValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.FatherName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.MotherName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.DOB)
                .NotEmpty()
                .LessThan(DateTime.Today);

            RuleFor(x => x.Age)
                .InclusiveBetween(1, 120);

            RuleFor(x => x.Gender)
                .NotEmpty()
                .MaximumLength(10);

            RuleFor(x => x.Field)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
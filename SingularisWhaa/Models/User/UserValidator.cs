using FluentValidation;

namespace SingularisWhaa.Models.User
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress();

            RuleFor(u => u.Name)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(u => u.Age)
                .InclusiveBetween(18, 99);
        }
    }
}

using Curotec.Application.DTOs;
using FluentValidation;

namespace Curotec.Application.Validations
{
    internal class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email can't be empty.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email is in invalid format");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can't be empty.");
        }
    }
}

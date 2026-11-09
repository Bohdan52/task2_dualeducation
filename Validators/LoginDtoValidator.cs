using FluentValidation;
using task2.Dtos.Auth;

namespace task2.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.UserNameOrEmail).NotEmpty().WithMessage("Логін або Email є обов'язковим.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Пароль є обов'язковим.");
    }
}
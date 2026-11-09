using FluentValidation;
using task2.Dtos.Auth;

namespace task2.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email є обов'язковим.")
            .EmailAddress().WithMessage("Неправильний формат Email.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username є обов'язковим.")
            .MinimumLength(3).WithMessage("Username має містити мінімум 3 символи.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль є обов'язковим.")
            .MinimumLength(6).WithMessage("Пароль має містити мінімум 6 символів.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Повне ім'я є обов'язковим.");
    }
}
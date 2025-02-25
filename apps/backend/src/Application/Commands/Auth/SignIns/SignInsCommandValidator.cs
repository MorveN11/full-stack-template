using FluentValidation;

namespace Application.Commands.Auth.SignIns;

internal sealed class SignInsCommandValidator : AbstractValidator<SignInsCommand>
{
    public SignInsCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}

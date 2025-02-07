using FluentValidation;

namespace Application.Commands.Auth.VerifyEmail;

internal sealed class VerifyUserEmailCommandValidator : AbstractValidator<VerifyUserEmailCommand>
{
    public VerifyUserEmailCommandValidator()
    {
        RuleFor(q => q.TokenId).NotEmpty();
    }
}

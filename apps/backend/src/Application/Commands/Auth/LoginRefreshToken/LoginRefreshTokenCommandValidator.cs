using FluentValidation;

namespace Application.Commands.Auth.LoginRefreshToken;

internal sealed class LoginRefreshTokenCommandValidator
    : AbstractValidator<LoginRefreshTokenCommand>
{
    public LoginRefreshTokenCommandValidator()
    {
        RuleFor(c => c.RefreshToken).NotEmpty();
    }
}

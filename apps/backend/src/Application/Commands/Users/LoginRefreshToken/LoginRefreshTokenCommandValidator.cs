using FluentValidation;

namespace Application.Commands.Users.LoginRefreshToken;

internal sealed class LoginRefreshTokenCommandValidator
    : AbstractValidator<LoginRefreshTokenCommand>
{
    public LoginRefreshTokenCommandValidator()
    {
        RuleFor(c => c.RefreshToken).NotEmpty();
    }
}

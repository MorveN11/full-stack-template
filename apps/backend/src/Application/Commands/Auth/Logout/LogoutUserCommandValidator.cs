using FluentValidation;

namespace Application.Commands.Auth.Logout;

internal sealed class LogoutUserCommandValidator : AbstractValidator<LogoutUserCommand>
{
    public LogoutUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

using FluentValidation;

namespace Application.Commands.Users.Logout;

internal sealed class LogoutUserCommandValidator : AbstractValidator<LogoutUserCommand>
{
    public LogoutUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

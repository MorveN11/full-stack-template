using FluentValidation;

namespace Application.Commands.Auth.FinishSessions;

internal sealed class FinishSessionsCommandValidator : AbstractValidator<FinishSessionsCommand>
{
    public FinishSessionsCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
    }
}

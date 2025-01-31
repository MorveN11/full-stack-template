using FluentValidation;

namespace Application.Commands.Users.RevokeRefreshTokens;

internal sealed class RevokeRefreshTokensCommandValidator
    : AbstractValidator<RevokeRefreshTokensCommand>
{
    public RevokeRefreshTokensCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

using FluentValidation;

namespace Application.Queries.Users.VerifyEmail;

internal sealed class VerifyUserEmailQueryValidator : AbstractValidator<VerifyUserEmailQuery>
{
    public VerifyUserEmailQueryValidator()
    {
        RuleFor(q => q.TokenId).NotEmpty();
    }
}

using FluentValidation;

namespace Application.Queries.Auth.GetSessionsById;

internal sealed class GetUserSessionsByIdQueryValidator
    : AbstractValidator<GetUserSessionsByIdQuery>
{
    public GetUserSessionsByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

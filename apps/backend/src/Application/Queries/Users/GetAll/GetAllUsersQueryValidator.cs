using FluentValidation;

namespace Application.Queries.Users.GetAll;

internal sealed class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        RuleFor(c => c.Page).GreaterThan(0);
        RuleFor(c => c.PageSize).GreaterThan(0);
    }
}

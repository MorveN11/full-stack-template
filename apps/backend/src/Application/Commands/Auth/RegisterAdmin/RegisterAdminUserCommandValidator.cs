using FluentValidation;

namespace Application.Commands.Auth.RegisterAdmin;

internal sealed class RegisterAdminUserCommandValidator
    : AbstractValidator<RegisterAdminUserCommand>
{
    private readonly HashSet<string> _validRoles = ["User", "Admin"];

    public RegisterAdminUserCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(8);

        RuleFor(c => c.Roles)
            .Must(roles => roles.Count > 0)
            .WithMessage("At least one role is required.")
            .ForEach(role =>
                role.NotNull()
                    .Must(BeAValidRole)
                    .WithMessage(
                        (_, r) =>
                            $"Invalid role '{r}'. Valid roles are: {string.Join(", ", _validRoles)}"
                    )
            );
    }

    private bool BeAValidRole(string role)
    {
        return _validRoles.Contains(role);
    }
}

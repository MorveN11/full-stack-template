using FluentValidation;

namespace Application.Commands.Auth.UpdatePassword;

internal sealed class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8);

        RuleFor(x => x.RefreshToken).NotEmpty();

        RuleFor(x => x.SignOutEverywhere).NotNull();
    }
}

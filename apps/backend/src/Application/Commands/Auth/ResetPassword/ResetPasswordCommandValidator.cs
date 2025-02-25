using FluentValidation;

namespace Application.Commands.Auth.ResetPassword;

internal sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(v => v.Email).NotEmpty().EmailAddress();

        RuleFor(v => v.OtpCode).NotEmpty().Length(6);

        RuleFor(v => v.NewPassword).NotEmpty().MinimumLength(8);
    }
}

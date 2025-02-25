using FluentValidation;

namespace Application.Commands.Auth.VerifyForgotPasswordOtpCode;

internal sealed class VerifyForgotPasswordOtpCodeCommandValidator
    : AbstractValidator<VerifyForgotPasswordOtpCodeCommand>
{
    public VerifyForgotPasswordOtpCodeCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.OtpCode).NotEmpty().Length(6);
    }
}

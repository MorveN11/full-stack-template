using FluentValidation;

namespace Application.Commands.Auth.VerifyRegisterOtpCode;

internal sealed class VerifyRegisterOtpCodeCommandValidator
    : AbstractValidator<VerifyRegisterOtpCodeCommand>
{
    public VerifyRegisterOtpCodeCommandValidator()
    {
        RuleFor(v => v.Email).NotEmpty().EmailAddress();

        RuleFor(v => v.OtpCode).NotEmpty().Length(6);
    }
}

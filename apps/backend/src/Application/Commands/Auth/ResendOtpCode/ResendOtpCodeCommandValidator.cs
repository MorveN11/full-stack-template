using Domain.Enums;
using FluentValidation;

namespace Application.Commands.Auth.ResendOtpCode;

internal sealed class ResendOtpCodeCommandValidator : AbstractValidator<ResendOtpCodeCommand>
{
    public ResendOtpCodeCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.CodeType)
            .NotEmpty()
            .IsEnumName(typeof(OtpCodeType))
            .WithMessage(
                "Invalid code type. Valid values are: "
                    + string.Join(", ", Enum.GetNames<OtpCodeType>())
            );
    }
}

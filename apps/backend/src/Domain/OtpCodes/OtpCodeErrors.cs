using SharedKernel.Errors;

namespace Domain.OtpCodes;

public static class OtpCodeErrors
{
    public static readonly Error NoPreviousOtpCode = Error.NotFound(
        "OtpCodes.NoPreviousOtpCode",
        "No previous OTP code was found"
    );

    public static readonly Error OtpCodeAlreadySent = Error.Conflict(
        "OtpCodes.AlreadySent",
        "The OTP code was already sent. Please wait for a minute before trying again"
    );

    public static readonly Error InvalidOtpCode = Error.Problem(
        "OtpCodes.InvalidOtpCode",
        "The provided OTP code is invalid"
    );
}

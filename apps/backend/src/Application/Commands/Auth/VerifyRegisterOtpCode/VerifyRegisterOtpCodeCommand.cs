using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.VerifyRegisterOtpCode;

public sealed record VerifyRegisterOtpCodeCommand(string Email, string OtpCode)
    : ICommand<VerifyRegisterOtpCodeResponse>;

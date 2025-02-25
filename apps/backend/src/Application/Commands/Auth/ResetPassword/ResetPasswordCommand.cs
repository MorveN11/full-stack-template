using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.ResetPassword;

public sealed record ResetPasswordCommand(string Email, string NewPassword, string OtpCode)
    : ICommand<ResetPasswordResponse>;

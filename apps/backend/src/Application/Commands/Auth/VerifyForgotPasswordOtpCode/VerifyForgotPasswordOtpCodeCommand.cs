using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.VerifyForgotPasswordOtpCode;

public sealed record VerifyForgotPasswordOtpCodeCommand(string Email, string OtpCode) : ICommand;

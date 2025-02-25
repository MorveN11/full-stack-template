using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.ResendOtpCode;

public sealed record ResendOtpCodeCommand(string Email, string CodeType) : ICommand;

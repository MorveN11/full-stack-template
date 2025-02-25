using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : ICommand;

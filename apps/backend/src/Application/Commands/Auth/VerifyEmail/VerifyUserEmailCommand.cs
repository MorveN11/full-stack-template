using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.VerifyEmail;

public sealed record VerifyUserEmailCommand(Guid TokenId) : ICommand;

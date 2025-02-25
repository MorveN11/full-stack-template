using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.FinishSessions;

public sealed record FinishSessionsCommand(Guid UserId) : ICommand;

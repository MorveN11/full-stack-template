using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.SignIns;

public sealed record SignInsCommand(string Email) : ICommand;

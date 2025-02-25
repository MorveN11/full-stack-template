using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;

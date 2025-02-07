using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<LoginUserResponse>;

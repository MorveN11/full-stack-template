using Application.Abstractions.Messaging;

namespace Application.Commands.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<LoginUserResponse>;

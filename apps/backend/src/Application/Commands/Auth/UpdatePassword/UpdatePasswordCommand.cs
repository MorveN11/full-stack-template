using Application.Abstractions.Messaging;

namespace Application.Commands.Auth.UpdatePassword;

public sealed record UpdatePasswordCommand(
    Guid UserId,
    string Email,
    string NewPassword,
    string RefreshToken,
    bool SignOutEverywhere
) : ICommand;

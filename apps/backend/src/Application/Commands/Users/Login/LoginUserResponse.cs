namespace Application.Commands.Users.Login;

public sealed record LoginUserResponse(string AccessToken, string RefreshToken);

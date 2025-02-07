namespace Application.Abstractions.Authentication;

public interface IUserContext
{
    Guid UserId { get; }

    string Email { get; }

    bool EmailVerified { get; }

    string Jwt { get; }

    List<string> Roles { get; }
}

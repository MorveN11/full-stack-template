using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId =>
        httpContextAccessor.HttpContext?.User.GetUserId()
        ?? throw new ApplicationException("User context is unavailable");

    public string Email =>
        httpContextAccessor.HttpContext?.User.GetEmail()
        ?? throw new ApplicationException("User context is unavailable");

    public bool EmailVerified =>
        httpContextAccessor.HttpContext?.User.GetEmailVerified()
        ?? throw new ApplicationException("User context is unavailable");

    public string Jwt =>
        httpContextAccessor.HttpContext?.Request.GetJwtToken()
        ?? throw new ApplicationException("User context is unavailable");

    public List<string> Roles =>
        httpContextAccessor.HttpContext?.User.GetRoles()
        ?? throw new ApplicationException("User context is unavailable");
}

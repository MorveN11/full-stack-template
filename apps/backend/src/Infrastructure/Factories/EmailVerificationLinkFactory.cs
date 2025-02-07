using Application.Abstractions.Factories;
using Domain.Identifiers;
using Domain.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Factories;

internal sealed class EmailVerificationLinkFactory(
    IHttpContextAccessor httpContextAccessor,
    LinkGenerator linkGenerator
) : IEmailVerificationLinkFactory
{
    public string Create(EmailVerificationToken emailVerificationToken)
    {
        string? verificationLink = linkGenerator.GetUriByName(
            httpContextAccessor.HttpContext!,
            Names.VerifyEmail,
            new { token = emailVerificationToken.Id }
        );

        return verificationLink
            ?? throw new Exception("Could not generate email verification link.");
    }
}

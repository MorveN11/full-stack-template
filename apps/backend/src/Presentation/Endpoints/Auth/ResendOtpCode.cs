using Application.Commands.Auth.ResendOtpCode;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class ResendOtpCode : IEndpoint
{
    public sealed record Request(string Email, string CodeType);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "auth/resend-otp-code",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new ResendOtpCodeCommand(request.Email, request.CodeType);

                    Result result = await sender.Send(query, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Auth)
            .WithSummary("Resend OTP code");
    }
}

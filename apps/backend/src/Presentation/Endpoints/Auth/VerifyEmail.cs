using Application.Commands.Auth.VerifyEmail;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class VerifyEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "auth/verify-email",
                async (Guid token, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new VerifyUserEmailCommand(token);

                    Result result = await sender.Send(query, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Auth)
            .WithName(Names.VerifyEmail)
            .WithSummary("Verify email");
    }
}

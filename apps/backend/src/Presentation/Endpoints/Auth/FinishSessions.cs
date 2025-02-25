using Application.Commands.Auth.FinishSessions;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class FinishSessions : IEndpoint
{
    public sealed record Request(Guid UserId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        _ = app.MapPost(
                "auth/finish-sessions",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new FinishSessionsCommand(request.UserId);

                    Result result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }
            )
            .RequireAuthorization()
            .WithTags(Tags.Auth)
            .WithSummary("Finish all user sessions");
    }
}

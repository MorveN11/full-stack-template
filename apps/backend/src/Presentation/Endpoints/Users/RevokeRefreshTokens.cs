using Application;
using Application.Commands.Users.RevokeRefreshTokens;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class RevokeRefreshTokens : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "users/{userId:guid}/refresh-tokens",
                async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new RevokeRefreshTokensCommand(userId);

                    Result result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Users)
            .RequireAuthorization()
            .WithSummary("Revoke refresh tokens");
    }
}

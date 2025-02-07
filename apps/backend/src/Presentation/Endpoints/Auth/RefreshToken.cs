using Application.Commands.Auth.LoginRefreshToken;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class RefreshToken : IEndpoint
{
    public sealed record Request(string RefreshToken);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "auth/refresh-token",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new LoginRefreshTokenCommand(request.RefreshToken);

                    Result<LoginRefreshTokenResponse> result = await sender.Send(
                        command,
                        cancellationToken
                    );

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Auth)
            .WithSummary("Refresh token");
    }
}

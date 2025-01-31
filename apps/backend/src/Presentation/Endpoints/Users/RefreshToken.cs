using Application;
using Application.Commands.Users.LoginRefreshToken;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class RefreshToken : IEndpoint
{
    public sealed class Request
    {
        public required string RefreshToken { get; init; }
    };

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "users/refresh-token",
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
            .WithTags(Tags.Users)
            .WithSummary("Refresh token");
    }
}
